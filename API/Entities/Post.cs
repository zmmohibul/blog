using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }
        
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public int UserId { get; set; }
        
        public User CreatedBy { get; set; }

        public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
    }
}