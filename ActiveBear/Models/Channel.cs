using System;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class Channel
    {
        // General details
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }

        // Encryption properties
        public string KeyHash { get; set; } // The key, hashed

        // Audit properties
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        private ActiveBearContext _context;

        public Channel(ActiveBearContext context)
        {
            Id = Guid.NewGuid();
            Status = Constants.Channel.Status.Active;
            CreateDate = DateTime.Now;
            _context = context;
        }
    }
}
