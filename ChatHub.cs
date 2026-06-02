using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace RealtimeChat.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> OnlineUsers = new ConcurrentDictionary<string, string>();

        public async Task SendMessage(string user, string message)
        {
            OnlineUsers[Context.ConnectionId] = user;
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (OnlineUsers.TryRemove(Context.ConnectionId, out string username))
            {
                await Clients.All.SendAsync("UserLeft", username);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
