using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authRepository.RegisterUser(registerDto);
            if (!result.IsSuccesful && result.StatusCode == 400) 
            {
                return BadRequest(new Error{StatusCode = 400, Message = result.ErrorMessage});
            }

            return CreatedAtRoute(nameof(GetUser), new { Username = result.Data.Username }, result.Data);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var result = await _authRepository.UserLogin(loginDto);

            if (!result.IsSuccesful && result.StatusCode == 401)
            {
                return Unauthorized(new Error{StatusCode = 401, Message = result.ErrorMessage});
            }

            return Ok(result.Data);
        }

        [Authorize]
        [HttpGet("allusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _authRepository.GetAllUsers();
            return Ok(result.Data);
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string username)
        {
            var result = await _authRepository.GetUserByUsername(username);

            if (result.IsSuccesful)
            {
                return Ok(result.Data);
            }

            // var u = User.FindFirst(ClaimTypes.Name);

            return NotFound(new Error{StatusCode = 404, Message = result.ErrorMessage});
        }

        [HttpGet("userexist")]
        public async Task<IActionResult> UserExists([FromQuery] string username)
        {
            if (await _authRepository.UsernameExists(username))
            {
                return Ok(new { Available = false });
            }
            else
            {
                return Ok(new { Available = true });
            }
        }
    }
}