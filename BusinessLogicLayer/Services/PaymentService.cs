using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Payment;
using BusinessLogicLayer.Services.Interfaces;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services;

public class PaymentService(
    ITutorBookingRepository bookingRepo,
    IPaymentRepository paymentRepo,
    IVnPayService vnPay,
    AppDbContext db,
    ILogger<PaymentService> logger
) : IPaymentService
{
    private const decimal HourlyRateFallback = 200_000m; // VND fallback nếu chưa set hourly rate

    // ─── Initiate Payment ────────────────────────────────────────────────────

    public async Task<ApiResponse<string>> InitiatePaymentAsync(
        long bookingId, long studentId, string clientIp, CancellationToken ct = default)
    {
        var booking = await bookingRepo.GetByIdAsync(bookingId, ct);

        if (booking is null)
            return ApiResponse<string>.Fail("Không tìm thấy booking.");

        if (booking.StudentId != studentId)
            return ApiResponse<string>.Fail("Bạn không có quyền thanh toán booking này.");

        if (booking.Status != BookingRequestStatus.AWAITING_PAYMENT)
            return ApiResponse<string>.Fail("Booking này chưa được giáo viên duyệt hoặc đã xử lý xong.");

        if (booking.PaymentDeadline.HasValue && booking.PaymentDeadline < DateTime.UtcNow)
            return ApiResponse<string>.Fail("Đã quá thời hạn thanh toán. Booking đã hết hạn.");

        // Kiểm tra đã có payment chưa
        var existingPayment = await paymentRepo.GetByBookingIdAsync(bookingId, ct);
        if (existingPayment?.Status == PaymentStatus.COMPLETED)
            return ApiResponse<string>.Fail("Booking này đã được thanh toán.");

        // Tính số tiền: dựa vào số giờ * hourly rate của teacher
        var duration = booking.RequestedEndTime - booking.RequestedStartTime;
        var hourlyRate = booking.Teacher.TeacherProfile?.HourlyRate ?? HourlyRateFallback;
        var amount = Math.Max((decimal)duration.TotalHours * hourlyRate, 1000m); // tối thiểu 1,000đ

        // Tạo unique transaction ref
        var vnpTxnRef = $"{bookingId}-{DateTime.UtcNow.Ticks}";

        // Tạo hoặc cập nhật payment record
        Payment payment;
        if (existingPayment is not null && existingPayment.Status == PaymentStatus.PENDING)
        {
            // Cập nhật TxnRef mới cho lần thử lại
            existingPayment.VnpTxnRef  = vnpTxnRef;
            existingPayment.Amount     = amount;
            existingPayment.UpdatedAt  = DateTime.UtcNow;
            await paymentRepo.UpdateAsync(existingPayment, ct);
            payment = existingPayment;
        }
        else
        {
            payment = new Payment
            {
                StudentId   = studentId,
                ClassId     = booking.ClassId ?? booking.ResultClassId ?? 0,
                BookingId   = bookingId,
                Amount      = amount,
                Type        = PaymentType.TUITION,
                Status      = PaymentStatus.PENDING,
                VnpTxnRef   = vnpTxnRef,
                ExpiredAt   = booking.PaymentDeadline,
                CreatedAt   = DateTime.UtcNow,
                UpdatedAt   = DateTime.UtcNow,
            };
            await paymentRepo.CreateAsync(payment, ct);
        }

        var orderInfo = $"Thanh toan khoa hoc booking {bookingId}";
        var paymentUrl = vnPay.CreatePaymentUrl(payment.Id, vnpTxnRef, amount, orderInfo, clientIp);

        return ApiResponse<string>.Ok(paymentUrl);
    }

    // ─── Process VNPay Return (user redirect) ────────────────────────────────

    public async Task<ApiResponse<PaymentResultDto>> ProcessReturnAsync(
        IEnumerable<KeyValuePair<string, string>> queryParams, CancellationToken ct = default)
    {
        var paramList = queryParams.ToList();

        var vnpTxnRef      = vnPay.GetParam(paramList, "vnp_TxnRef");
        var responseCode   = vnPay.GetParam(paramList, "vnp_ResponseCode");
        var secureHash     = vnPay.GetParam(paramList, "vnp_SecureHash");
        var amountStr      = vnPay.GetParam(paramList, "vnp_Amount");
        var transactionNo  = vnPay.GetParam(paramList, "vnp_TransactionNo");
        var bankCode       = vnPay.GetParam(paramList, "vnp_BankCode");

        if (string.IsNullOrEmpty(vnpTxnRef) || string.IsNullOrEmpty(secureHash))
            return ApiResponse<PaymentResultDto>.Fail("Tham số không hợp lệ từ VNPay.");

        var isValid = vnPay.ValidateSignature(paramList, secureHash);
        if (!isValid)
            return ApiResponse<PaymentResultDto>.Fail("Chữ ký không hợp lệ.");

        var payment = await paymentRepo.GetByVnpTxnRefAsync(vnpTxnRef, ct);
        if (payment is null)
            return ApiResponse<PaymentResultDto>.Fail("Không tìm thấy giao dịch.");

        var amount  = decimal.TryParse(amountStr, out var a) ? a / 100 : payment.Amount;
        var success = responseCode == "00";

        var bookingIdStr = vnpTxnRef.Split('-')[0];

        var result = new PaymentResultDto(
            Success:       success,
            Message:       success ? "Thanh toán thành công!" : $"Thanh toán thất bại (Mã lỗi: {responseCode})",
            BookingId:     bookingIdStr,
            VnpTxnRef:    vnpTxnRef,
            TransactionNo: transactionNo,
            BankCode:      bankCode,
            Amount:        amount
        );

        // Nếu thành công → xử lý luôn (cho trường hợp IPN không đến được, localhost dev)
        if (success && payment.Status == PaymentStatus.PENDING)
        {
            await FinalizeSuccessfulPaymentAsync(payment, transactionNo, bankCode, ct);
        }
        else if (!success && payment.Status == PaymentStatus.PENDING)
        {
            payment.Status    = PaymentStatus.FAILED;
            payment.UpdatedAt = DateTime.UtcNow;
            await paymentRepo.UpdateAsync(payment, ct);

            // Cập nhật booking nếu cần
            if (long.TryParse(bookingIdStr, out var bId))
            {
                var booking = await bookingRepo.GetByIdAsync(bId, ct);
                if (booking is { Status: BookingRequestStatus.AWAITING_PAYMENT })
                {
                    // Giữ AWAITING_PAYMENT — có thể thử lại nếu chưa hết deadline
                }
            }
        }

        return success ? ApiResponse<PaymentResultDto>.Ok(result) : ApiResponse<PaymentResultDto>.Fail(result.Message);
    }

    // ─── Process VNPay IPN (server-to-server) ────────────────────────────────

    public async Task<(bool ok, string message)> ProcessIpnAsync(
        IEnumerable<KeyValuePair<string, string>> queryParams, CancellationToken ct = default)
    {
        var paramList = queryParams.ToList();

        var vnpTxnRef    = vnPay.GetParam(paramList, "vnp_TxnRef");
        var responseCode = vnPay.GetParam(paramList, "vnp_ResponseCode");
        var secureHash   = vnPay.GetParam(paramList, "vnp_SecureHash");
        var transNo      = vnPay.GetParam(paramList, "vnp_TransactionNo");
        var bankCode     = vnPay.GetParam(paramList, "vnp_BankCode");
        var amountStr    = vnPay.GetParam(paramList, "vnp_Amount");

        if (!vnPay.ValidateSignature(paramList, secureHash ?? ""))
        {
            logger.LogWarning("VNPay IPN invalid signature for TxnRef={Ref}", vnpTxnRef);
            return (false, "97"); // Invalid signature
        }

        var payment = await paymentRepo.GetByVnpTxnRefAsync(vnpTxnRef ?? "", ct);
        if (payment is null)
        {
            logger.LogWarning("VNPay IPN: Payment not found TxnRef={Ref}", vnpTxnRef);
            return (false, "01"); // Order not found
        }

        if (payment.Status != PaymentStatus.PENDING)
        {
            logger.LogInformation("VNPay IPN: Payment {Id} already processed", payment.Id);
            return (true, "00"); // Already confirmed
        }

        // Verify amount
        var expectedAmount = (long)(payment.Amount * 100);
        if (long.TryParse(amountStr, out var receivedAmount) && receivedAmount != expectedAmount)
        {
            logger.LogWarning("VNPay IPN amount mismatch. Expected={E}, Got={G}", expectedAmount, receivedAmount);
            return (false, "04"); // Invalid amount
        }

        if (responseCode == "00")
        {
            await FinalizeSuccessfulPaymentAsync(payment, transNo, bankCode, ct);
        }
        else
        {
            payment.Status    = PaymentStatus.FAILED;
            payment.UpdatedAt = DateTime.UtcNow;
            await paymentRepo.UpdateAsync(payment, ct);
        }

        return (true, "00");
    }

    // ─── Get Student Bookings ────────────────────────────────────────────────

    public async Task<ApiResponse<IReadOnlyList<BookingPaymentSummaryDto>>> GetStudentBookingsAsync(
        long studentId, CancellationToken ct = default)
    {
        var bookings = await bookingRepo.GetByStudentWithClassAsync(studentId, ct);
        var dtos = bookings.Select(b =>
        {
            var canPay = b.Status == BookingRequestStatus.AWAITING_PAYMENT
                      && (!b.PaymentDeadline.HasValue || b.PaymentDeadline > DateTime.UtcNow);

            // Tính amount (giờ * hourly rate)
            decimal? amount = null;
            if (b.Status == BookingRequestStatus.AWAITING_PAYMENT || b.Status == BookingRequestStatus.PAYMENT_SUCCESS || b.Status == BookingRequestStatus.BOOKING_SUCCESS)
            {
                var duration   = b.RequestedEndTime - b.RequestedStartTime;
                var hourlyRate = b.Teacher.TeacherProfile?.HourlyRate ?? HourlyRateFallback;
                amount = Math.Round((decimal)duration.TotalHours * hourlyRate, 0);
            }

            return new BookingPaymentSummaryDto(
                BookingId:                b.Id,
                TeacherName:              b.Teacher.TeacherProfile?.FullName ?? b.Teacher.Email,
                TeacherAvatar:            b.Teacher.TeacherProfile?.AvatarUrl ?? b.Teacher.AvatarUrl,
                Status:                   b.Status.ToString(),
                ClassName:                b.ResultClass?.Name,
                ClassId:                  b.ClassId ?? b.ResultClassId,
                Amount:                   amount,
                RequestedStartTimeLocal:  b.RequestedStartTime.ToLocalTime(),
                RequestedEndTimeLocal:    b.RequestedEndTime.ToLocalTime(),
                PaymentDeadlineLocal:     b.PaymentDeadline?.ToLocalTime(),
                CreatedAtLocal:           b.CreatedAt.ToLocalTime(),
                CancelReason:             b.CancelReason,
                CanPay:                   canPay
            );
        }).ToList();

        return ApiResponse<IReadOnlyList<BookingPaymentSummaryDto>>.Ok(dtos);
    }

    // ─── Private: Finalize success ───────────────────────────────────────────

    private async Task FinalizeSuccessfulPaymentAsync(
        Payment payment, string? transactionNo, string? bankCode, CancellationToken ct)
    {
        // 1. Cập nhật Payment
        payment.Status          = PaymentStatus.COMPLETED;
        payment.VnpTransactionNo = transactionNo;
        payment.BankCode        = bankCode;
        payment.PaidAt          = DateTime.UtcNow;
        payment.UpdatedAt       = DateTime.UtcNow;
        await paymentRepo.UpdateAsync(payment, ct);

        // 2. Lấy booking
        if (!payment.BookingId.HasValue) return;
        var booking = await bookingRepo.GetByIdAsync(payment.BookingId.Value, ct);
        if (booking is null) return;

        // 3. Cập nhật booking PAYMENT_SUCCESS trước
        booking.Status    = BookingRequestStatus.PAYMENT_SUCCESS;
        booking.UpdatedAt = DateTime.UtcNow;
        await bookingRepo.UpdateAsync(booking, ct);

        // 4. Auto-join class
        var classId = booking.ClassId ?? booking.ResultClassId;
        if (classId.HasValue && classId.Value > 0)
        {
            var alreadyMember = await db.ClassMembers
                .AnyAsync(m => m.ClassId == classId.Value && m.StudentId == booking.StudentId, ct);

            if (!alreadyMember)
            {
                db.ClassMembers.Add(new ClassMember
                {
                    ClassId   = classId.Value,
                    StudentId = booking.StudentId,
                    Status    = ClassMemberStatus.ACTIVE,
                    JoinedAt  = DateTime.UtcNow,
                });
                await db.SaveChangesAsync(ct);
            }

            // 5. Cập nhật booking BOOKING_SUCCESS
            booking.Status    = BookingRequestStatus.BOOKING_SUCCESS;
            booking.UpdatedAt = DateTime.UtcNow;
            await bookingRepo.UpdateAsync(booking, ct);
        }

        // 6. Gửi notification cho student
        await SendPaymentSuccessNotification(booking.StudentId, payment.Amount, ct);

        logger.LogInformation("Payment {PayId} finalized. Booking {BkId} → BOOKING_SUCCESS", payment.Id, booking.Id);
    }

    private async Task SendPaymentSuccessNotification(long studentId, decimal amount, CancellationToken ct)
    {
        try
        {
            db.Notifications.Add(new Notification
            {
                UserId    = studentId,
                Title     = "Thanh toán thành công! 🎉",
                Content   = $"Bạn đã thanh toán {amount:N0}đ thành công. Bạn đã được tự động thêm vào lớp học. Chúc bạn học vui vẻ!",
                IsRead    = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });
            await db.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send payment success notification to student {Id}", studentId);
        }
    }
}
