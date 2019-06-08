using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ActiveBear.Models
{
    public class Channel
    {
        // General details
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<User> AuthorisedUsers { get; set; } //TODO: should probably be a method on ChannelAuths...
        public string Status { get; set; }

        // Encryption properties
        public string KeyHash { get; set; } // The key, hashed

        // Audit properties
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }
        public User CreateUser { get; set; }
        private ActiveBearContext _context;

        public Channel(ActiveBearContext context)
        {
            Id = Guid.NewGuid();
            AuthorisedUsers = new List<User>();
            Status = "ACTIVE"; //TODO: global constant this
            CreateDate = DateTime.Now;
            _context = context;
        }

        public List<Message> GetMessages()
        {
            return _context.Messages.Where(m => m.Channel == Id).ToList();
        }

        public int MemberCount()
        {
            return AuthorisedUsers.Count;
        }
    }
}
