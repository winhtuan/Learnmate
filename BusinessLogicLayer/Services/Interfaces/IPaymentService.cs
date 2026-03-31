using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.DTOs.Payment;

namespace BusinessLogicLayer.Services.Interfaces;

public interface IPaymentService
{
    /// <summary>Student khởi tạo thanh toán — trả về URL redirect sang VNPay</summary>
    Task<ApiResponse<string>> InitiatePaymentAsync(long bookingId, long studentId, string clientIp, CancellationToken ct = default);

    /// <summary>Xử lý VNPay ReturnUrl (user redirect về sau khi qua cổng)</summary>
    Task<ApiResponse<PaymentResultDto>> ProcessReturnAsync(IEnumerable<KeyValuePair<string, string>> queryParams, CancellationToken ct = default);

    /// <summary>Xử lý VNPay IPN callback (server-to-server, authoritative)</summary>
    Task<(bool ok, string message)> ProcessIpnAsync(IEnumerable<KeyValuePair<string, string>> queryParams, CancellationToken ct = default);

    /// <summary>Lấy danh sách booking của student (kèm thông tin payment)</summary>
    Task<ApiResponse<IReadOnlyList<BookingPaymentSummaryDto>>> GetStudentBookingsAsync(long studentId, CancellationToken ct = default);
}
