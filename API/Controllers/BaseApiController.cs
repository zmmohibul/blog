using API.Dtos;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult HandleUnsuccessfulResult<T>(Result<T> result)
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

            return BadRequest(error);
        }
        
    }

    
}