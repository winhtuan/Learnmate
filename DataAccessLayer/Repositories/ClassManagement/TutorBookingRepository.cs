using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class TutorBookingRepository(AppDbContext db) : ITutorBookingRepository
{
    public async Task<TutorBookingRequest> CreateAsync(TutorBookingRequest request, CancellationToken ct = default)
    {
        db.TutorBookingRequests.Add(request);
        await db.SaveChangesAsync(ct);
        return request;
    }

    public async Task<IReadOnlyList<TutorBookingRequest>> GetByStudentIdAsync(long studentId, CancellationToken ct = default) =>
        await db.TutorBookingRequests
            .AsNoTracking()
            .Where(r => r.StudentId == studentId && r.DeletedAt == null)
            .Include(r => r.Teacher).ThenInclude(t => t.TeacherProfile)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);

    public async Task<TutorBookingRequest?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await db.TutorBookingRequests
            .Include(r => r.Teacher).ThenInclude(t => t.TeacherProfile)
            .Include(r => r.Student).ThenInclude(s => s.StudentProfile)
            .FirstOrDefaultAsync(r => r.Id == id && r.DeletedAt == null, ct);

    public async Task UpdateAsync(TutorBookingRequest request, CancellationToken ct = default)
    {
        db.TutorBookingRequests.Update(request);
        await db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<TutorBookingRequest>> GetExpiredAwaitingPaymentAsync(CancellationToken ct = default) =>
        await db.TutorBookingRequests
            .Where(r => r.Status == BookingRequestStatus.AWAITING_PAYMENT
                     && r.PaymentDeadline != null
                     && r.PaymentDeadline < DateTime.UtcNow
                     && r.DeletedAt == null)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<TutorBookingRequest>> GetByStudentWithClassAsync(long studentId, CancellationToken ct = default) =>
        await db.TutorBookingRequests
            .AsNoTracking()
            .Where(r => r.StudentId == studentId && r.DeletedAt == null)
            .Include(r => r.Teacher).ThenInclude(t => t.TeacherProfile)
            .Include(r => r.ResultClass)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);
}
