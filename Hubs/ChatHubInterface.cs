using System;
using System.Threading.Tasks;

namespace ActiveBear.Hubs
{
    public interface IChatHub
    {
        Task CurrentUser(string name);

        Task ReceiveMessage(string message);

        Task ReceiveAllMessages(string messages);

        Task Notification(string notification);

        Task ChannelCreated(Guid channelId);
    }
}
