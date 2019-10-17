using System;
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
        public Guid UserCookie = Guid.Empty;

        [DataMember]
        public Guid Channel = Guid.Empty;

        [DataMember]
        public string Message = string.Empty;
    }

    // ChannelInfoPacket is sent from the JS when all messages for a channel are requested.
    // Server uses this to find all messages for a channel and check the user has auth.
    [DataContract]
    class ChannelInfoPacket
    {
        [DataMember]
        public Guid UserCookie = Guid.Empty;

        [DataMember]
        public Guid Channel = Guid.Empty;
    }

    // ChannelCreationPacket holds the info necessary to create a channel. Note that fields can be empty.
    [DataContract]
    class ChannelCreationPacket
    {
        [DataMember]
        public Guid UserCookie = Guid.Empty;

        [DataMember]
        public string ChannelTitle = string.Empty;

        [DataMember]
        public string ChannelKey = string.Empty;
    }

    // CookiePacket contains the user cookie of the current user.
    // We can process this to lookup the relevant User in the database.
    [DataContract]
    class CookiePacket
    {
        [DataMember]
        public Guid UserCookie = Guid.Empty;
    }
}
