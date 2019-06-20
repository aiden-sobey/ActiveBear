using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ActiveBear.Services;

namespace ActiveBear.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            var context = DbService.NewDbContext();
            var messageCreate = MessageService.NewMessage(context.Users.FirstOrDefault(), context.Channels.FirstOrDefault(), message);
            
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
