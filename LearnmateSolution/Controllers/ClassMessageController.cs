using System.Security.Claims;
using BusinessLogicLayer.DTOs.Messaging;
using BusinessLogicLayer.Services.Interfaces.ClassManagement;
using LearnmateSolution.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LearnmateSolution.Controllers;

[ApiController]
[Route("api/classes/{id:long}/messages")]
[Authorize]
public class ClassMessageController(
    IClassMessageService classMessageService,
    IHubContext<ChatHub> hubContext) : ControllerBase
{
    private long? UserId =>
        long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

    /// <summary>GET /api/classes/{id}/messages — Lấy tin nhắn nhóm lớp.</summary>
    [HttpGet]
    public async Task<IActionResult> GetMessages(
        long id,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50,
        CancellationToken ct = default)
    {
        if (UserId is not { } userId) return Unauthorized();
        var result = await classMessageService.GetClassMessagesAsync(id, userId, skip, take, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>POST /api/classes/{id}/messages — Gửi tin nhắn vào nhóm lớp.</summary>
    [HttpPost]
    public async Task<IActionResult> SendMessage(long id, [FromBody] SendClassMessageDto dto, CancellationToken ct)
    {
        if (UserId is not { } userId) return Unauthorized();
        var result = await classMessageService.SendClassMessageAsync(id, userId, dto, ct);
        if (!result.Success) return BadRequest(result);

        // Push real-time to class group
        await hubContext.Clients
            .Group($"class_{id}")
            .SendAsync("ReceiveClassMessage", result.Data, ct);

        return Ok(result);
    }
}
