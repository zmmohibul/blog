using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.StatusCode == 400) 
            {
                return BadRequest(new Error{StatusCode = 400, Message = result.ErrorMessage});
            }

            return Ok(result.Data);
        }
        
    }
}