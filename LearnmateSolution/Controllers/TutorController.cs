using System.Security.Claims;
using BusinessLogicLayer.DTOs.Tutor;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnmateSolution.Controllers;

[ApiController]
[Authorize]
public class TutorController(ITutorService tutorService) : ControllerBase
{
    private long? UserId =>
        long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

    /// <summary>GET /api/tutors — Danh sách giáo viên có TeacherProfile.</summary>
    [HttpGet("api/tutors")]
    public async Task<IActionResult> GetTutors(
        [FromQuery] string? subject,
        [FromQuery] decimal? maxRate,
        CancellationToken ct)
    {
        var result = await tutorService.GetTutorsAsync(subject, maxRate, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>POST /api/tutors/{teacherId}/book — Tạo booking request.</summary>
    [HttpPost("api/tutors/{teacherId:long}/book")]
    public async Task<IActionResult> Book(long teacherId, [FromBody] CreateBookingRequestDto dto, CancellationToken ct)
    {
        if (UserId is not { } studentId)
            return Unauthorized();

        dto.TeacherId = teacherId;
        var result = await tutorService.CreateBookingRequestAsync(studentId, dto, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>GET /api/bookings/my — Lịch booking của student hiện tại.</summary>
    [HttpGet("api/bookings/my")]
    public async Task<IActionResult> GetMyBookings(CancellationToken ct)
    {
        if (UserId is not { } studentId)
            return Unauthorized();

        var result = await tutorService.GetStudentBookingsAsync(studentId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>DELETE /api/bookings/{id} — Student hủy booking (chỉ PENDING).</summary>
    [HttpDelete("api/bookings/{id:long}")]
    public async Task<IActionResult> CancelBooking(long id, CancellationToken ct)
    {
        if (UserId is not { } studentId)
            return Unauthorized();

        var result = await tutorService.CancelBookingAsync(id, studentId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
