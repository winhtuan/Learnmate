using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces.ClassManagement;

public interface IClassMessageRepository
{
    Task<List<ClassMessage>> GetClassMessagesAsync(long classId, int skip, int take, CancellationToken ct);
    Task<bool> AddAsync(ClassMessage message, CancellationToken ct);
}
