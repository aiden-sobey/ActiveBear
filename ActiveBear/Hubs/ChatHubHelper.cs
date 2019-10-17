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
    }
}
