using System;
using Newtonsoft.Json;

namespace ActiveBear.Hubs
{
    public static class ChatHubHelper
    {
        public static string GroupFor(string encodedPacket)
        {
            var packet = JsonConvert.DeserializeObject<ChannelInfoPacket>(encodedPacket);
            return packet.Channel.ToString();
        }

        public static string BuildError(string contents)
        {
            return BuildNotification(Constants.NotificationTypes.Error, contents);
        }

        public static string BuildInfo(string contents)
        {
            return BuildNotification(Constants.NotificationTypes.Info, contents);
        }

        private static string BuildNotification(string code, string contents)
        {
            var notification = new Notification
            {
                Type = code,
                Contents = contents
            };

            return JsonConvert.SerializeObject(notification);
        }
    }
}
