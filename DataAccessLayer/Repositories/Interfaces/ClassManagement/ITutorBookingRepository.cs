using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface ITutorBookingRepository
{
    Task<TutorBookingRequest> CreateAsync(TutorBookingRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<TutorBookingRequest>> GetByStudentIdAsync(long studentId, CancellationToken ct = default);
    Task<TutorBookingRequest?> GetByIdAsync(long id, CancellationToken ct = default);
    Task UpdateAsync(TutorBookingRequest request, CancellationToken ct = default);
    /// <summary>Lấy các booking AWAITING_PAYMENT đã hết hạn (PaymentDeadline < now)</summary>
    Task<IReadOnlyList<TutorBookingRequest>> GetExpiredAwaitingPaymentAsync(CancellationToken ct = default);
    /// <summary>Lấy booking của student kèm class info</summary>
    Task<IReadOnlyList<TutorBookingRequest>> GetByStudentWithClassAsync(long studentId, CancellationToken ct = default);
}
