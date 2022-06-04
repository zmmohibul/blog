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

            return Ok(result.Data);
        }

    }
}