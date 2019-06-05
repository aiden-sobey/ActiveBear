using System;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Sender { get; set; }
        public Guid Channel { get; set; }
        public string EncryptedContents { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SendDate { get; set; }

        public Message() { }
    }
}
