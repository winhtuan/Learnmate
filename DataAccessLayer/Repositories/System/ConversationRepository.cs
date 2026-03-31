using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories.System;

public class ConversationRepository(AppDbContext db) : IConversationRepository
{
    // Include participants + their profiles for name resolution
    private static IQueryable<Conversation> WithParticipants(IQueryable<Conversation> q) =>
        q.Include(c => c.ParticipantA).ThenInclude(u => u.TeacherProfile)
         .Include(c => c.ParticipantA).ThenInclude(u => u.StudentProfile)
         .Include(c => c.ParticipantB).ThenInclude(u => u.TeacherProfile)
         .Include(c => c.ParticipantB).ThenInclude(u => u.StudentProfile);

    public async Task<Conversation> GetOrCreateAsync(long userAId, long userBId, CancellationToken ct = default)
    {
        var (aId, bId) = (Math.Min(userAId, userBId), Math.Max(userAId, userBId));

        var existing = await WithParticipants(db.Conversations)
            .Where(c => c.ParticipantAId == aId && c.ParticipantBId == bId && c.DeletedAt == null)
            .FirstOrDefaultAsync(ct);

        if (existing is not null) return existing;

        var conv = new Conversation { ParticipantAId = aId, ParticipantBId = bId };
        db.Conversations.Add(conv);

        try
        {
            await db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            // Race condition: another request created the same conversation
            db.Entry(conv).State = EntityState.Detached;
            existing = await db.Conversations
                .Include(c => c.ParticipantA)
                .Include(c => c.ParticipantB)
                .Where(c => c.ParticipantAId == aId && c.ParticipantBId == bId && c.DeletedAt == null)
                .FirstAsync(ct);
            return existing;
        }

        // Reload with participants + profiles
        await db.Entry(conv).Reference(c => c.ParticipantA).LoadAsync(ct);
        await db.Entry(conv).Reference(c => c.ParticipantB).LoadAsync(ct);
        if (conv.ParticipantA.TeacherProfile is null)
            await db.Entry(conv.ParticipantA).Reference(u => u.TeacherProfile).LoadAsync(ct);
        if (conv.ParticipantA.StudentProfile is null)
            await db.Entry(conv.ParticipantA).Reference(u => u.StudentProfile).LoadAsync(ct);
        if (conv.ParticipantB.TeacherProfile is null)
            await db.Entry(conv.ParticipantB).Reference(u => u.TeacherProfile).LoadAsync(ct);
        if (conv.ParticipantB.StudentProfile is null)
            await db.Entry(conv.ParticipantB).Reference(u => u.StudentProfile).LoadAsync(ct);
        return conv;
    }

    public async Task<IReadOnlyList<Conversation>> GetByUserIdAsync(long userId, CancellationToken ct = default)
    {
        return await WithParticipants(db.Conversations.AsNoTracking())
            .Include(c => c.Messages.OrderByDescending(m => m.CreatedAt).Take(1))
            .Where(c => (c.ParticipantAId == userId || c.ParticipantBId == userId) && c.DeletedAt == null)
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Conversation?> GetWithMessagesAsync(
        long conversationId, long requestingUserId,
        int skip = 0, int take = 50,
        CancellationToken ct = default)
    {
        var conv = await WithParticipants(db.Conversations.AsNoTracking())
            .Where(c => c.Id == conversationId && c.DeletedAt == null &&
                        (c.ParticipantAId == requestingUserId || c.ParticipantBId == requestingUserId))
            .FirstOrDefaultAsync(ct);

        if (conv is null) return null;

        var messages = await db.Messages
            .AsNoTracking()
            .Where(m => m.ConversationId == conversationId)
            .OrderByDescending(m => m.CreatedAt)
            .Skip(skip).Take(take)
            .ToListAsync(ct);

        messages.Reverse(); // Display oldest-first
        conv.Messages = messages;
        return conv;
    }

    public async Task<Message> AddMessageAsync(Message message, CancellationToken ct = default)
    {
        db.Messages.Add(message);

        // Update LastMessageAt on conversation
        await db.Conversations
            .Where(c => c.Id == message.ConversationId)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.LastMessageAt, DateTime.UtcNow), ct);

        await db.SaveChangesAsync(ct);

        // Load sender for mapping
        await db.Entry(message).Reference(m => m.Sender).LoadAsync(ct);
        return message;
    }

    public async Task MarkReadAsync(long conversationId, long readByUserId, CancellationToken ct = default)
    {
        await db.Messages
            .Where(m => m.ConversationId == conversationId &&
                        m.SenderId != readByUserId &&
                        !m.IsRead)
            .ExecuteUpdateAsync(s => s
                .SetProperty(m => m.IsRead, true)
                .SetProperty(m => m.ReadAt, DateTime.UtcNow), ct);
    }

    public async Task<int> CountUnreadAsync(long conversationId, long userId, CancellationToken ct = default)
    {
        return await db.Messages
            .CountAsync(m => m.ConversationId == conversationId &&
                             m.SenderId != userId &&
                             !m.IsRead, ct);
    }
}
