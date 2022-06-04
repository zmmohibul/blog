using System;

namespace API.Dtos
{
    public class PostToReturnDto
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; }
    }
}