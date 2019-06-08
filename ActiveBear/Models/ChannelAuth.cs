using System;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class ChannelAuth
    {
        [Key]
        public Guid Id { get; set; }
        public string User { get; set; }
        public Guid Channel { get; set; }
        public string UserEncryptedKey { get; set; }

        public string RawKey(string userPassword)
        {
            // TODO: decryption here
            return UserEncryptedKey;
        }
    }
}
