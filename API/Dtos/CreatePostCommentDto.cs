using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CreatePostCommentDto
    {
        [Required]
        public string Content { get; set; }
    }
}