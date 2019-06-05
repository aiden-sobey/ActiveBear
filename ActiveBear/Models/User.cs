using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public List<ChannelAuth> ChannelAuths { get; set; }

        public User() { }
    }
}
