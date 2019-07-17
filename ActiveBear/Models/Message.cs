using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActiveBear.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        [Required][ForeignKey("UserForeignKey")]
        public string Sender { get; set; }

        [Required][ForeignKey("ChannelForeignKey")]
        public Guid Channel { get; set; }

        [Required]
        public string EncryptedContents { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SendDate { get; set; }

        public Message()
        {
            Id = Guid.NewGuid();
            SendDate = DateTime.Now;
        }
    }
}
