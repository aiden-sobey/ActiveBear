using System;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class ChannelAuth
    {
        [Key]
        public Guid Id { get; set; }
        public Guid User { get; set; }
        public Guid Channel { get; set; }
        [Required]
        public string HashedKey { get; set; }

        public ChannelAuth()
        {
            Id = Guid.NewGuid();
        }
    }
}
