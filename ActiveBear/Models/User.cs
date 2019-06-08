using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Password)][Required]
        public string Password { get; set; }
        public List<ChannelAuth> ChannelAuths { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
            ChannelAuths = new List<ChannelAuth>();
        }
    }
}
