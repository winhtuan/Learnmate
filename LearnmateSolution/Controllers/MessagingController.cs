using System.Security.Claims;
using BusinessLogicLayer.DTOs.Messaging;
using BusinessLogicLayer.Services.Interfaces;
using LearnmateSolution.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace LearnmateSolution.Controllers;

[ApiController]
[Authorize]
public class MessagingController(
    IMessagingService messagingService,
    IHubContext<ChatHub> hubContext) : ControllerBase
{
    private long? UserId =>
        long.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

    /// <summary>POST /api/conversations/{otherUserId} — Get or create conversation.</summary>
    [HttpPost("api/conversations/{otherUserId:long}")]
    public async Task<IActionResult> GetOrCreate(long otherUserId, CancellationToken ct)
    {
        if (UserId is not { } userId) return Unauthorized();
        var result = await messagingService.GetOrCreateConversationAsync(userId, otherUserId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>GET /api/conversations — Danh sách conversations của user.</summary>
    [HttpGet("api/conversations")]
    public async Task<IActionResult> GetConversations(CancellationToken ct)
    {
        if (UserId is not { } userId) return Unauthorized();
        var result = await messagingService.GetConversationsAsync(userId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>GET /api/conversations/{id}/messages — Lấy messages (query: skip, take).</summary>
    [HttpGet("api/conversations/{id:long}/messages")]
    public async Task<IActionResult> GetMessages(
        long id,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50,
        CancellationToken ct = default)
    {
        if (UserId is not { } userId) return Unauthorized();
        var result = await messagingService.GetMessagesAsync(id, userId, skip, take, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    /// <summary>POST /api/conversations/{id}/messages — Gửi message.</summary>
    [HttpPost("api/conversations/{id:long}/messages")]
    public async Task<IActionResult> SendMessage(long id, [FromBody] SendMessageDto dto, CancellationToken ct)
    {
        if (UserId is not { } userId) return Unauthorized();
        var result = await messagingService.SendMessageAsync(id, userId, dto, ct);
        if (!result.Success) return BadRequest(result);

        // Push real-time to conversation group
        await hubContext.Clients
            .Group($"conv_{id}")
            .SendAsync("ReceiveMessage", result.Data, ct);

        return Ok(result);
    }

    /// <summary>POST /api/conversations/{id}/read — Mark messages as read.</summary>
    [HttpPost("api/conversations/{id:long}/read")]
    public async Task<IActionResult> MarkRead(long id, CancellationToken ct)
    {
        if (UserId is not { } userId) return Unauthorized();
        var result = await messagingService.MarkReadAsync(id, userId, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
