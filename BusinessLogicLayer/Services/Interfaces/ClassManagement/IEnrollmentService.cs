using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Payment;

namespace BusinessLogicLayer.Services.Interfaces.ClassManagement;

public interface IEnrollmentService
{
    /// <summary>
    /// Student bấm Enroll → tạo Invoice PENDING → trả invoiceId để redirect PaymentPage.
    /// Guard: student chưa enroll lớp, lớp ACTIVE, không có invoice PENDING trùng.
    /// </summary>
    Task<ApiResponse<CreateInvoiceResponseDto>> CreateInvoiceAsync(long studentId, long classId, CancellationToken ct = default);

    /// <summary>
    /// Lấy thông tin invoice để PaymentPage hiển thị.
    /// Chỉ trả về nếu invoice thuộc về studentId.
    /// </summary>
    Task<ApiResponse<InvoiceDetailDto>> GetInvoiceDetailAsync(long invoiceId, long studentId, CancellationToken ct = default);

    /// <summary>
    /// Xác nhận thanh toán.
    /// SUCCESS → Invoice=PAID, tạo ClassMember ACTIVE trong 1 transaction.
    /// FAILED  → Payment=FAILED, Invoice giữ PENDING (cho phép retry).
    /// </summary>
    Task<ApiResponse<ConfirmPaymentResponseDto>> ConfirmPaymentAsync(ConfirmPaymentRequestDto request, long studentId, CancellationToken ct = default);

    /// <summary>Tạo Payment record + trả về URL VNPay sandbox để redirect.</summary>
    Task<ApiResponse<string>> InitiateVnPayAsync(long invoiceId, long studentId, string clientIp, CancellationToken ct = default);

    /// <summary>Student tự huỷ invoice (chỉ khi Status == PENDING).</summary>
    Task<ApiResponse<bool>> CancelInvoiceAsync(long invoiceId, long studentId, CancellationToken ct = default);
}
