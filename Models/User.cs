using System;
using System.ComponentModel.DataAnnotations;

namespace ActiveBear.Models
{
    public class User
    {
        [Key]
        public string Name { get; set; }

        [Required, DataType(DataType.Password), MinLength(12)]
        public string Password { get; set; }

        public Guid CookieId { get; set; }

        public string Description { get; set; }

        public User()
        {
            CookieId = Guid.NewGuid();
        }
    }
}
