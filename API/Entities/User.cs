using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace API.Entities
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }
        
        public byte[] PasswordHash { get; set; }
        
        public byte[] PasswordSalt { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();

    }
}