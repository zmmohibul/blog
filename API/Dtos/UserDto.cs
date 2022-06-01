using System;

namespace API.Dtos
{
    public class UserDto
    {
        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Token { get; set; }
    }
}