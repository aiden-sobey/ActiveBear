using System;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid Sender { get; set; }
        [Required]
        public Guid Channel { get; set; }
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
