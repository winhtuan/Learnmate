using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface ITutorBookingRepository
{
    Task<TutorBookingRequest> CreateAsync(TutorBookingRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<TutorBookingRequest>> GetByStudentIdAsync(long studentId, CancellationToken ct = default);
    Task<TutorBookingRequest?> GetByIdAsync(long id, CancellationToken ct = default);
    Task UpdateAsync(TutorBookingRequest request, CancellationToken ct = default);
}
