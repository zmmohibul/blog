using System;

namespace API.Entities
{
    public class Post
    {
        public int Id { get; set; }
        
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public int UserId { get; set; }
        
        public User CreatedBy { get; set; }
        
    }
}