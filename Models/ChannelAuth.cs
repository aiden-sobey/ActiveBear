using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActiveBear.Models
{
    public class ChannelAuth
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("UserForeignKey")]
        public string User { get; set; }

        [ForeignKey("ChannelForeignKey")]
        public Guid Channel { get; set; }

        [Required]
        public string HashedKey { get; set; }

        public ChannelAuth()
        {
            Id = Guid.NewGuid();
        }
    }
}
