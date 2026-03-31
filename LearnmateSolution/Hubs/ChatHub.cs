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

    public async Task StopTyping(long conversationId) =>
        await Clients.OthersInGroup($"conv_{conversationId}")
            .SendAsync("UserTyping", false);
}
