using BusinessObject.Models;

namespace DataAccessLayer.Repositories.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> CreateAsync(Payment payment, CancellationToken ct = default);
    Task<Payment?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<Payment?> GetByVnpTxnRefAsync(string vnpTxnRef, CancellationToken ct = default);
    Task<Payment?> GetByBookingIdAsync(long bookingId, CancellationToken ct = default);
    Task UpdateAsync(Payment payment, CancellationToken ct = default);
}
