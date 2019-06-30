using System.Runtime.Serialization;

// This file contains serializable class definitions for the objects
// passed to-from the JavaScript.
namespace ActiveBear.Hubs
{
    // MessagePacket is sent from JS when a message is sent, and contains data
    // neccessary for the server to construct a Message object.
    [DataContract]
    class MessagePacket
    {
        [DataMember]
        public string UserCookie = string.Empty;

        [DataMember]
        public string Channel = string.Empty;

        [DataMember]
        public string Message = string.Empty;
    }

    // ChannelInfoPacket is sent from the JS when all messages for a channel are requested.
    // Server uses this to find all messages for a channel and check the user has auth.
    [DataContract]
    class ChannelInfoPacket
    {
        [DataMember]
        public string UserCookie = string.Empty;

        [DataMember]
        public string Channel = string.Empty;
    }

    // ChannelCreationPacket holds the info necessary to create a channel. Note that fields can be empty.
    [DataContract]
    class ChannelCreationPacket
    {
        [DataMember]
        public string UserCookie = string.Empty;

        [DataMember]
        public string ChannelTitle = string.Empty;

        [DataMember]
        public string ChannelKey = string.Empty;
    }
}
