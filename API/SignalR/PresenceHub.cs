using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _presenceTracker;
        public PresenceHub(PresenceTracker presenceTracker)
        {
            _presenceTracker = presenceTracker;

        }

        public override async Task OnConnectedAsync()
        {
            await _presenceTracker.UserConnected(Context.User.FindFirst(ClaimTypes.Name).Value, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.FindFirst(ClaimTypes.Name).Value);

            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _presenceTracker.UserDisconnected(Context.User.FindFirst(ClaimTypes.Name).Value, Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User.FindFirst(ClaimTypes.Name).Value);

            var currentUsers = await _presenceTracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }
    }
}