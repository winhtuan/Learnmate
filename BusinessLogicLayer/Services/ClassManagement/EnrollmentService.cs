using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Payment;
using BusinessLogicLayer.Services.Interfaces;
using BusinessLogicLayer.Services.Interfaces.ClassManagement;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services.ClassManagement;

public class EnrollmentService(AppDbContext db, IVnPayService vnPay) : IEnrollmentService
{
    public async Task<ApiResponse<CreateInvoiceResponseDto>> CreateInvoiceAsync(
        long studentId, long classId, CancellationToken ct = default)
    {
        var cls = await db.Classes
            .Include(c => c.Teacher).ThenInclude(t => t.TeacherProfile)
            .FirstOrDefaultAsync(c => c.Id == classId && c.DeletedAt == null && c.Status == ClassStatus.ACTIVE, ct);

        if (cls is null)
            return ApiResponse<CreateInvoiceResponseDto>.Fail("Lớp học không tồn tại hoặc không còn mở.");

        if (cls.Price <= 0)
            return ApiResponse<CreateInvoiceResponseDto>.Fail("Lớp học này miễn phí, vui lòng đăng ký trực tiếp.");

        var alreadyEnrolled = await db.ClassMembers
            .AnyAsync(m => m.ClassId == classId && m.StudentId == studentId, ct);
        if (alreadyEnrolled)
            return ApiResponse<CreateInvoiceResponseDto>.Fail("Bạn đã tham gia lớp học này rồi.");

        // Trả về invoice PENDING cũ nếu có — tránh tạo trùng
        var existing = await db.Invoices
            .FirstOrDefaultAsync(i =>
                i.StudentId == studentId &&
                i.ClassId   == classId   &&
                i.Status    == InvoiceStatus.PENDING &&
                i.DeletedAt == null, ct);

        var teacherName = cls.Teacher.TeacherProfile?.FullName ?? cls.Teacher.Email;

        if (existing is not null)
            return ApiResponse<CreateInvoiceResponseDto>.Ok(
                new CreateInvoiceResponseDto(existing.Id, cls.Id, cls.Name, teacherName, existing.TotalAmount));

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var invoice = new Invoice
        {
            TeacherId   = cls.TeacherId,
            StudentId   = studentId,
            ClassId     = classId,
            TotalAmount = cls.Price,
            Status      = InvoiceStatus.PENDING,
            IssuedAt    = DateTime.UtcNow,
            PeriodStart = today,
            PeriodEnd   = today.AddDays(1),
        };

        db.Invoices.Add(invoice);
        await db.SaveChangesAsync(ct);

        return ApiResponse<CreateInvoiceResponseDto>.Ok(
            new CreateInvoiceResponseDto(invoice.Id, cls.Id, cls.Name, teacherName, invoice.TotalAmount));
    }

    public async Task<ApiResponse<InvoiceDetailDto>> GetInvoiceDetailAsync(
        long invoiceId, long studentId, CancellationToken ct = default)
    {
        var invoice = await db.Invoices
            .Include(i => i.Class)
                .ThenInclude(c => c.Teacher).ThenInclude(t => t.TeacherProfile)
            .FirstOrDefaultAsync(i =>
                i.Id        == invoiceId  &&
                i.StudentId == studentId  &&
                i.DeletedAt == null, ct);

        if (invoice is null)
            return ApiResponse<InvoiceDetailDto>.Fail("Không tìm thấy hoá đơn.");

        var teacherName = invoice.Class.Teacher.TeacherProfile?.FullName
                          ?? invoice.Class.Teacher.Email;

        return ApiResponse<InvoiceDetailDto>.Ok(new InvoiceDetailDto(
            invoice.Id,
            invoice.ClassId,
            invoice.Class.Name,
            teacherName,
            null,
            invoice.TotalAmount,
            invoice.Status));
    }

    public async Task<ApiResponse<ConfirmPaymentResponseDto>> ConfirmPaymentAsync(
        ConfirmPaymentRequestDto request, long studentId, CancellationToken ct = default)
    {
        await using var tx = await db.Database.BeginTransactionAsync(ct);
        try
        {
            var invoice = await db.Invoices
                .Include(i => i.Class)
                .FirstOrDefaultAsync(i =>
                    i.Id        == request.InvoiceId &&
                    i.StudentId == studentId         &&
                    i.DeletedAt == null, ct);

            if (invoice is null)
                return ApiResponse<ConfirmPaymentResponseDto>.Fail("Không tìm thấy hoá đơn.");

            if (invoice.Status != InvoiceStatus.PENDING)
                return ApiResponse<ConfirmPaymentResponseDto>.Fail(
                    $"Hoá đơn ở trạng thái {invoice.Status}, không thể thanh toán.");

            // Mock gateway: bất kỳ TransactionId nào khác "FAILED" đều thành công
            // Thực tế: gọi VNPay/MoMo SDK verify tại đây
            var isSuccess = !request.TransactionId.Equals("FAILED", StringComparison.OrdinalIgnoreCase);

            var payment = new Payment
            {
                StudentId      = studentId,
                ClassId        = invoice.ClassId,
                InvoiceId      = invoice.Id,
                Amount         = invoice.TotalAmount,
                Type           = PaymentType.TUITION,
                Method         = request.Method,
                Status         = isSuccess ? PaymentStatus.SUCCESS : PaymentStatus.FAILED,
                VnpTxnRef      = request.TransactionId,
                FailureReason  = isSuccess ? null : "Giao dịch bị từ chối.",
            };

            db.Payments.Add(payment);

            if (isSuccess)
            {
                invoice.Status = InvoiceStatus.PAID;
                invoice.PaidAt = DateTime.UtcNow;

                // Invariant: ClassMember chỉ được tạo tại đây sau khi thanh toán SUCCESS
                db.ClassMembers.Add(new ClassMember
                {
                    ClassId   = invoice.ClassId,
                    StudentId = studentId,
                    Status    = ClassMemberStatus.ACTIVE,
                    JoinedAt  = DateTime.UtcNow,
                });
            }

            await db.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return isSuccess
                ? ApiResponse<ConfirmPaymentResponseDto>.Ok(
                    new ConfirmPaymentResponseDto(true, invoice.ClassId, "Thanh toán thành công. Chào mừng bạn đến lớp học!"))
                : ApiResponse<ConfirmPaymentResponseDto>.Ok(
                    new ConfirmPaymentResponseDto(false, null, "Thanh toán thất bại. Vui lòng thử lại."));
        }
        catch
        {
            await tx.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<ApiResponse<string>> InitiateVnPayAsync(
        long invoiceId, long studentId, string clientIp, CancellationToken ct = default)
    {
        var invoice = await db.Invoices
            .Include(i => i.Class)
            .FirstOrDefaultAsync(i =>
                i.Id        == invoiceId &&
                i.StudentId == studentId &&
                i.DeletedAt == null, ct);

        if (invoice is null)
            return ApiResponse<string>.Fail("Không tìm thấy hoá đơn.");

        if (invoice.Status != InvoiceStatus.PENDING)
            return ApiResponse<string>.Fail($"Hoá đơn ở trạng thái {invoice.Status}, không thể thanh toán.");

        var vnpTxnRef = $"inv{invoiceId}-{DateTime.UtcNow.Ticks}";

        // Reuse existing PENDING payment or create new
        var existingPayment = await db.Payments
            .FirstOrDefaultAsync(p => p.InvoiceId == invoiceId && p.Status == PaymentStatus.PENDING, ct);

        Payment payment;
        if (existingPayment is not null)
        {
            existingPayment.VnpTxnRef = vnpTxnRef;
            existingPayment.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync(ct);
            payment = existingPayment;
        }
        else
        {
            payment = new Payment
            {
                StudentId = studentId,
                ClassId   = invoice.ClassId,
                InvoiceId = invoiceId,
                Amount    = invoice.TotalAmount,
                Type      = PaymentType.TUITION,
                Method    = PaymentMethod.VNPAY,
                Status    = PaymentStatus.PENDING,
                VnpTxnRef = vnpTxnRef,
            };
            db.Payments.Add(payment);
            await db.SaveChangesAsync(ct);
        }

        var className = invoice.Class.Name;
        var orderInfo = $"Thanh toan {className[..Math.Min(30, className.Length)]}";
        var url = vnPay.CreatePaymentUrl(payment.Id, vnpTxnRef, invoice.TotalAmount, orderInfo, clientIp);
        return ApiResponse<string>.Ok(url);
    }

    public async Task<ApiResponse<bool>> CancelInvoiceAsync(
        long invoiceId, long studentId, CancellationToken ct = default)
    {
        var invoice = await db.Invoices
            .FirstOrDefaultAsync(i =>
                i.Id        == invoiceId &&
                i.StudentId == studentId &&
                i.DeletedAt == null, ct);

        if (invoice is null)
            return ApiResponse<bool>.Fail("Không tìm thấy hoá đơn.");

        if (invoice.Status != InvoiceStatus.PENDING)
            return ApiResponse<bool>.Fail("Chỉ có thể huỷ hoá đơn đang chờ thanh toán.");

        invoice.Status = InvoiceStatus.CANCELLED;
        await db.SaveChangesAsync(ct);

        return ApiResponse<bool>.Ok(true);
    }
}
