using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using APIGateway.Entities;

namespace APIGateway.Services.MessagingService
{
    public class MessageHub : Hub
    {
        private string _eventCommandGroup = "events_commands";
        private string _notifyMethod = "new_data";

        public async Task Notify(EventCommand data)
        {
            await Clients.Group(_eventCommandGroup).SendAsync(_notifyMethod, data);
        }

        public async Task<string> JoinGroup()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, _eventCommandGroup);
            return "Joined group " + _eventCommandGroup;
        }

        public async Task<string> LeaveGroup()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _eventCommandGroup);
            return "Left group " + _eventCommandGroup;
        }
    }
}
