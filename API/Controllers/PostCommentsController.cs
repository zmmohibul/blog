using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PostCommentsController : BaseApiController
    {
        private readonly IPostCommentRepository _postCommentRepository;
        public PostCommentsController(IPostCommentRepository postCommentRepository)
        {
            _postCommentRepository = postCommentRepository;
        }

        [HttpPost("post/{postId}")]
        public async Task<IActionResult> CreatePostComment(int postId, CreatePostCommentDto createPostCommentDto) 
        {
            var result = await _postCommentRepository.CreateComment(postId, createPostCommentDto, User.FindFirst(ClaimTypes.Name).Value);
            if (!result.IsSuccesful) 
            {
                return HandleUnsuccessfulResult(result);
            }

            return Ok(result.Data);
        }

    }
}