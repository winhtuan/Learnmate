namespace BusinessLogicLayer.DTOs.Payment;

/// <summary>Kết quả sau khi VNPay redirect về</summary>
public record PaymentResultDto(
    bool    Success,
    string  Message,
    string  BookingId,
    string  VnpTxnRef,
    string? TransactionNo,
    string? BankCode,
    decimal Amount
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
