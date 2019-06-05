using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class Channel
    {
        // General details
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public List<Message> Messages { get; set; }
        public List<User> AuthorisedUsers { get; set; }
        public long MemberCount { get; set; }
        public string Status { get; set; }

        // Encryption properties
        public string KeyHash { get; set; } // The key, hashed

        // Audit properties
        [DataType(DataType.DateTime)]
        public DateTime CreateDate { get; set; }
        public User CreateUser { get; set; }

        public Channel()
        { }

        // Methods


    }
}
