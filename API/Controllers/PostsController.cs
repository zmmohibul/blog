using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PostsController : BaseApiController
    {
        private readonly IPostRepository _postRepository;
        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            return Ok((await _postRepository.GetAllPostsAsync()).Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var result = await _postRepository.GetPostByIdAsync(id);

            if (!result.IsSuccesful)
            {
                var error = new Error{StatusCode = result.StatusCode, Message = result.ErrorMessage};
                if (result.StatusCode == 404)
                {
                    return NotFound(error);
                }
            }

            return Ok(result.Data);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto) 
        {
            var result = await _postRepository.CreatePostAsync(createPostDto, User.FindFirst(ClaimTypes.Name).Value);
        
            return Ok(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDto updatePostDto) 
        {
            var result = await _postRepository.UpdatePostAsync(id, User.FindFirst(ClaimTypes.Name).Value, updatePostDto); 
            
            if (!result.IsSuccesful) 
            {
                return HandleUnsuccessfulResult(result);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id) 
        {
            var result = await _postRepository.DeletePostAsync(id, User.FindFirst(ClaimTypes.Name).Value); 
            
            if (!result.IsSuccesful) 
            {
                return HandleUnsuccessfulResult(result);
            }

            return NoContent();
        }


        private IActionResult HandleUnsuccessfulResult(Result<PostToReturnDto> result)
        {
            if (!result.IsSuccesful) 
            {
                var error = new Error{StatusCode = result.StatusCode, Message = result.ErrorMessage};
                if (result.StatusCode == 404)
                {
                    return NotFound(error);
                }

                if (result.StatusCode == 401)
                {
                    return Unauthorized(error);
                }
            }

            return Ok();
        }

    }
}