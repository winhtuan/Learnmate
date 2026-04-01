using BusinessObject.Enum;

namespace BusinessLogicLayer.DTOs.Payment;

/// <summary>Kết quả sau khi VNPay redirect về</summary>
public record PaymentResultDto(
    bool    Success,
    string  Message,
    string  BookingId,
    string  VnpTxnRef,
    string? TransactionNo,
    string? BankCode,
    decimal Amount,
    long?   ClassId = null
);

/// <summary>Tổng quan booking + payment status cho Student</summary>
public record BookingPaymentSummaryDto(
    long    BookingId,
    string  TeacherName,
    string? TeacherAvatar,
    string  Status,           // BookingRequestStatus enum string
    string? ClassName,        // Tên class sẽ join (nếu đã có)
    long?   ClassId,
    decimal? Amount,          // Số tiền cần thanh toán
    DateTime RequestedStartTimeLocal,
    DateTime RequestedEndTimeLocal,
    DateTime? PaymentDeadlineLocal,  // Hạn chót thanh toán
    DateTime CreatedAtLocal,
    string? CancelReason,
    bool    CanPay            // true nếu status == AWAITING_PAYMENT và chưa hết hạn
);

/// <summary>Body request khi student khởi tạo thanh toán</summary>
public record InitiatePaymentRequestDto(
    long BookingId
);

// ─── Enrollment flow ──────────────────────────────────────────────────────────

/// <summary>Trả về cho client sau khi tạo invoice — dùng để redirect sang PaymentPage</summary>
public record CreateInvoiceResponseDto(
    long    InvoiceId,
    long    ClassId,
    string  ClassName,
    string  TeacherName,
    decimal Amount
);

/// <summary>Thông tin đầy đủ của invoice — PaymentPage dùng để hiển thị</summary>
public record InvoiceDetailDto(
    long          InvoiceId,
    long          ClassId,
    string        ClassName,
    string        TeacherName,
    string?       Schedule,
    decimal       Amount,
    InvoiceStatus Status
);

/// <summary>Request body khi student xác nhận thanh toán</summary>
public record ConfirmPaymentRequestDto(
    long          InvoiceId,
    PaymentMethod Method,
    /// <summary>Mock: "SUCCESS" hoặc "FAILED". Thực tế: mã từ cổng thanh toán.</summary>
    string        TransactionId
);

/// <summary>Kết quả sau khi confirm thanh toán</summary>
public record ConfirmPaymentResponseDto(
    bool   Success,
    long?  ClassId,
    string Message
);
