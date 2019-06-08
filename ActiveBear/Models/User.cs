using System;
using System.Collections.Generic;
using ActiveBear.Messages;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class User
    {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required, DataType(DataType.Password), MinLength(8)]
        public string Password { get; set; }

        public User() { }

        public List<ChannelAuth> GetAuthorisedChannels()
        {
            return new List<ChannelAuth>();
        }
    }
}
