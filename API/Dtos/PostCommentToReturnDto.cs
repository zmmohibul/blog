using System.Collections.Generic;

namespace API.Dtos
{
    public class PostCommentToReturnDto
    {
        public int Count { get; set; }
        public List<PostCommentDto> Comments { get; set; } = new List<PostCommentDto>();
    }
}