using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using API.Entities;

namespace API.Dtos
{
    public class PostToReturnDto
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public int NumberOfComments { get; set; }
        
        public ICollection<PostCommentDto> Comments { get; set; }
    }
}