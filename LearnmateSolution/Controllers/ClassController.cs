using System.Security.Claims;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnmateSolution.Controllers;

[ApiController]
[Route("api/classes")]
[Authorize]
public class ClassController(IClassService classService) : ControllerBase
{
    private long? UserId =>
        long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

    /// <summary>GET /api/classes — Returns all enrolled classes for the current student.</summary>
    [HttpGet]
    public async Task<IActionResult> GetEnrolled(CancellationToken ct)
    {
        if (UserId is not { } userId)
            return Unauthorized();
        var result = await classService.GetEnrolledClassesAsync(userId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>GET /api/classes/{id} — Returns full class detail for the current student.</summary>
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetDetail(long id, CancellationToken ct)
    {
        if (UserId is not { } userId)
            return Unauthorized();
        var result = await classService.GetClassDetailAsync(id, userId, ct);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>GET /api/classes/{id}/assignments — Returns all assignments with submission status.</summary>
    [HttpGet("{id:long}/assignments")]
    public async Task<IActionResult> GetAssignments(long id, CancellationToken ct)
    {
        if (UserId is not { } userId)
            return Unauthorized();
        var result = await classService.GetClassAssignmentsAsync(id, userId, ct);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>GET /api/classes/{id}/schedules — Returns all non-cancelled schedules.</summary>
    [HttpGet("{id:long}/schedules")]
    public async Task<IActionResult> GetSchedules(long id, CancellationToken ct)
    {
        if (UserId is not { } userId)
            return Unauthorized();
        var result = await classService.GetClassSchedulesAsync(id, userId, ct);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>GET /api/classes/{id}/materials — Returns all active materials.</summary>
    [HttpGet("{id:long}/materials")]
    public async Task<IActionResult> GetMaterials(long id, CancellationToken ct)
    {
        if (UserId is not { } userId)
            return Unauthorized();
        var result = await classService.GetClassMaterialsAsync(id, userId, ct);
        return result.Success ? Ok(result) : NotFound(result);
    }

    /// <summary>DELETE /api/classes/{id}/members/me — Student leaves the class.</summary>
    [HttpDelete("{id:long}/members/me")]
    public async Task<IActionResult> LeaveClass(long id, CancellationToken ct)
    {
        if (UserId is not { } userId)
            return Unauthorized();
        var result = await classService.LeaveClassAsync(id, userId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>POST /api/classes/{id}/assignments/{asgId}/submit — Submit an assignment (optional file).</summary>
    [HttpPost("{id:long}/assignments/{asgId:long}/submit")]
    public async Task<IActionResult> SubmitAssignment(
        long id,
        long asgId,
        IFormFile? file,
        CancellationToken ct
    )
    {
        if (UserId is not { } userId)
            return Unauthorized();

        Stream? stream = file?.OpenReadStream();
        var result = await classService.SubmitAssignmentAsync(
            id,
            asgId,
            userId,
            stream,
            file?.FileName,
            file?.ContentType,
            ct
        );
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>POST /api/classes/{id}/materials — Upload a material file.</summary>
    [HttpPost("{id:long}/materials")]
    public async Task<IActionResult> UploadMaterial(
        long id,
        [FromForm] string title,
        IFormFile file,
        CancellationToken ct
    )
    {
        if (UserId is not { } userId)
            return Unauthorized();
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Title is required.");

        await using var stream = file.OpenReadStream();
        var result = await classService.UploadMaterialAsync(
            id,
            userId,
            title,
            stream,
            file.FileName,
            file.ContentType,
            ct
        );
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>GET /api/classes/{id}/videos — Returns live and recorded video sessions.</summary>
    [HttpGet("{id:long}/videos")]
    public async Task<IActionResult> GetVideos(long id, CancellationToken ct)
    {
        if (UserId is not { } userId)
            return Unauthorized();
        var result = await classService.GetClassVideosAsync(id, userId, ct);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
