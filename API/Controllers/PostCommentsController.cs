using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
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

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetComments(int postId, [FromQuery] QueryParameters queryParameters) 
        {
            var result = await _postCommentRepository.GetComments(postId, queryParameters);
            if (!result.IsSuccesful)
            {
                return HandleUnsuccessfulResult(result);
            }

            return Ok(result.Data);
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