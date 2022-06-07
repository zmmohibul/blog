using System;

namespace API.Dtos
{
    public class PostCommentDto
    {
        public int Id { get; set; }
        
        public string Content { get; set; }
        
        public int PostId { get; set; }

        public string Username { get; set; }
        
        public DateTime CreatedAt { get; set; }
    }
}