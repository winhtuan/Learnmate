using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class PaymentRepository(AppDbContext db) : IPaymentRepository
{
    public async Task<Payment> CreateAsync(Payment payment, CancellationToken ct = default)
    {
        db.Payments.Add(payment);
        await db.SaveChangesAsync(ct);
        return payment;
    }

    public async Task<Payment?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await db.Payments
            .Include(p => p.Student)
            .Include(p => p.Class)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Payment?> GetByVnpTxnRefAsync(string vnpTxnRef, CancellationToken ct = default) =>
        await db.Payments
            .Include(p => p.Student)
            .Include(p => p.Class)
            .FirstOrDefaultAsync(p => p.VnpTxnRef == vnpTxnRef, ct);

    public async Task<Payment?> GetByBookingIdAsync(long bookingId, CancellationToken ct = default) =>
        await db.Payments
            .FirstOrDefaultAsync(p => p.BookingId == bookingId, ct);

    public async Task UpdateAsync(Payment payment, CancellationToken ct = default)
    {
        db.Payments.Update(payment);
        await db.SaveChangesAsync(ct);
    }
}
