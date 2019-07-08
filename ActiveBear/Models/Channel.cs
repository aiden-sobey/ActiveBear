using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActiveBear.Models
{
    public class Channel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string KeyHash { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }

        [ForeignKey("UserForeignKey")]
        public string CreateUser { get; set; }

        public string Status { get; set; }

        public Channel()
        {
            Id = Guid.NewGuid();
            Status = Constants.Channel.Status.Active;
            CreateDate = DateTime.Now;
        }
    }
}
