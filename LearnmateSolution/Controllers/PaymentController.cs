using System.Security.Claims;
using BusinessLogicLayer.DTOs.Payment;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnmateSolution.Controllers;

[ApiController]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    private long? UserId =>
        long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

    /// <summary>
    /// POST /api/payment/initiate — Student khởi tạo thanh toán VNPay
    /// Returns: URL redirect sang cổng VNPay
    /// </summary>
    [HttpPost("api/payment/initiate")]
    [Authorize]
    public async Task<IActionResult> Initiate([FromBody] InitiatePaymentRequestDto dto, CancellationToken ct)
    {
        if (UserId is not { } studentId) return Unauthorized();

        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        // Behind proxy, use X-Forwarded-For
        if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            clientIp = forwardedFor.ToString().Split(',')[0].Trim();

        var result = await paymentService.InitiatePaymentAsync(dto.BookingId, studentId, clientIp, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>
    /// POST /api/payment/vnpay-ipn — VNPay IPN server-to-server callback
    /// Phải public (không cần auth) vì VNPay gọi trực tiếp
    /// </summary>
    [HttpPost("api/payment/vnpay-ipn")]
    [AllowAnonymous]
    public async Task<IActionResult> VnPayIpn(CancellationToken ct)
    {
        var queryParams = Request.Query
            .Select(q => new KeyValuePair<string, string>(q.Key, q.Value.ToString()))
            .ToList();

        var (ok, code) = await paymentService.ProcessIpnAsync(queryParams, ct);

        // VNPay yêu cầu trả về {"RspCode":"00","Message":"Confirm Success"}
        return Ok(new { RspCode = ok ? "00" : code, Message = ok ? "Confirm Success" : "Error" });
    }

    /// <summary>
    /// GET /api/payment/vnpay-return-api — API endpoint cho Blazor gọi khi xử lý return
    /// </summary>
    [HttpGet("api/payment/vnpay-return-api")]
    [AllowAnonymous]
    public async Task<IActionResult> VnPayReturnApi(CancellationToken ct)
    {
        var queryParams = Request.Query
            .Select(q => new KeyValuePair<string, string>(q.Key, q.Value.ToString()))
            .ToList();

        var result = await paymentService.ProcessReturnAsync(queryParams, ct);
        return result.Success ? Ok(result) : Ok(result); // Luôn trả 200, Blazor tự xử lý
    }

    /// <summary>
    /// GET /api/bookings/my — Danh sách booking của student hiện tại (có status payment)
    /// </summary>
    [HttpGet("api/bookings/my/payment")]
    [Authorize]
    public async Task<IActionResult> GetMyBookingsWithPayment(CancellationToken ct)
    {
        if (UserId is not { } studentId) return Unauthorized();
        var result = await paymentService.GetStudentBookingsAsync(studentId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
