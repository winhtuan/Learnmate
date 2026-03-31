using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LearnmateSolution.Hubs;

[Authorize]
public class ChatHub : Hub
{
    public async Task JoinConversation(long conversationId) =>
        await Groups.AddToGroupAsync(Context.ConnectionId, $"conv_{conversationId}");

    public async Task LeaveConversation(long conversationId) =>
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conv_{conversationId}");

    public async Task StartTyping(long conversationId) =>
        await Clients.OthersInGroup($"conv_{conversationId}")
            .SendAsync("UserTyping", true);

    public async Task JoinClassConversation(long classId) =>
        await Groups.AddToGroupAsync(Context.ConnectionId, $"class_{classId}");

    public async Task LeaveClassConversation(long classId) =>
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"class_{classId}");

    public async Task StartClassTyping(long classId, string userName) =>
        await Clients.OthersInGroup($"class_{classId}")
            .SendAsync("ClassUserTyping", userName, true);

    public async Task StopClassTyping(long classId, string userName) =>
        await Clients.OthersInGroup($"class_{classId}")
            .SendAsync("ClassUserTyping", userName, false);
}
