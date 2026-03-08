using System.Security.Claims;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnmateSolution.Controllers;

[ApiController]
[Route("api/student")]
[Authorize]
public class StudentDashboardController(IStudentDashboardService dashboardService) : ControllerBase
{
    /// <summary>Returns the student's dashboard data (stats, schedule, classes, notifications).</summary>
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await dashboardService.GetDashboardAsync(userId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>Returns the minimal profile data needed by the header (name, avatar, grade).</summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile(CancellationToken ct)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!long.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var result = await dashboardService.GetProfileHeaderAsync(userId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
