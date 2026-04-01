using System.Security.Claims;
using BusinessLogicLayer.DTOs.Payment;
using BusinessLogicLayer.Services.Interfaces.ClassManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnmateSolution.Controllers;

[ApiController]
[Authorize]
[Route("api/enrollment")]
public class EnrollmentController(IEnrollmentService enrollmentService) : ControllerBase
{
    private long? UserId =>
        long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

    /// <summary>POST api/enrollment/create-invoice — Student bấm Enroll → tạo Invoice → trả invoiceId</summary>
    [HttpPost("create-invoice")]
    public async Task<IActionResult> CreateInvoice([FromBody] long classId, CancellationToken ct)
    {
        if (UserId is not { } studentId) return Unauthorized();

        var result = await enrollmentService.CreateInvoiceAsync(studentId, classId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>GET api/enrollment/invoice/{invoiceId} — PaymentPage lấy thông tin hoá đơn</summary>
    [HttpGet("invoice/{invoiceId:long}")]
    public async Task<IActionResult> GetInvoiceDetail(long invoiceId, CancellationToken ct)
    {
        if (UserId is not { } studentId) return Unauthorized();

        var result = await enrollmentService.GetInvoiceDetailAsync(invoiceId, studentId, ct);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>POST api/enrollment/confirm — Student xác nhận thanh toán</summary>
    [HttpPost("confirm")]
    public async Task<IActionResult> ConfirmPayment([FromBody] ConfirmPaymentRequestDto request, CancellationToken ct)
    {
        if (UserId is not { } studentId) return Unauthorized();

        var result = await enrollmentService.ConfirmPaymentAsync(request, studentId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>POST api/enrollment/{invoiceId}/initiate-vnpay — Tạo URL VNPay sandbox để redirect</summary>
    [HttpPost("{invoiceId:long}/initiate-vnpay")]
    public async Task<IActionResult> InitiateVnPay(long invoiceId, CancellationToken ct)
    {
        if (UserId is not { } studentId) return Unauthorized();

        var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        if (HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
            clientIp = forwardedFor.ToString().Split(',')[0].Trim();

        var result = await enrollmentService.InitiateVnPayAsync(invoiceId, studentId, clientIp, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>POST api/enrollment/invoice/{invoiceId}/cancel — Student huỷ hoá đơn PENDING</summary>
    [HttpPost("invoice/{invoiceId:long}/cancel")]
    public async Task<IActionResult> CancelInvoice(long invoiceId, CancellationToken ct)
    {
        if (UserId is not { } studentId) return Unauthorized();

        var result = await enrollmentService.CancelInvoiceAsync(invoiceId, studentId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
