using System;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class ChannelAuth
    {
        [Key]
        public Guid Id { get; set; }
        public User User { get; set; }
        public Channel Channel { get; set; }
        public string UserEncryptedKey { get; set; }

        public ChannelAuth() { }

        public string RawKey(string userPassword)
        {
            // TODO: decryption here
            return UserEncryptedKey;
        }
    }
}
