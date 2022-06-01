using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        public AuthController(DataContext context)
        {
            _context = context;

        }

        [HttpGet("allusers")]
        public async Task<IActionResult> GetAllUsers() 
        {
            return Ok(await _context.Users.ToListAsync());
        }

    }
}