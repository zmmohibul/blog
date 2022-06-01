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
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public AuthController(DataContext context, ITokenService tokenService, IMapper mapper)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _context = context;

        }

        [Authorize]
        [HttpGet("allusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(string username)
        {
            if (!await UserExists(username)) {
                return BadRequest(new Error {statusCode = 400, Message = $"No user exists with username {username}"});
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username.Equals(username));

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var userExist = await _context.Users.AnyAsync(x => x.Username.Equals(registerDto.Username.ToLower()));
            if (await UserExists(registerDto.Username)) 
            {
                return BadRequest(new Error {statusCode = 400, Message = "Username is taken"});
            }

            var user = _mapper.Map<User>(registerDto);

            using var hmac = new HMACSHA512();

            user.Username = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            return CreatedAtRoute(nameof(GetUser), new { Username = user.Username }, GetUserDto(user));
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Username.Equals(loginDto.Username));

            if (user == null) return Unauthorized(new Error {statusCode = 401, Message = "Invalid Username"});

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized(new Error {statusCode = 401, Message = "Invalid Password"});
            }

            return Ok(GetUserDto(user));
        }

        [HttpGet("userexist")]
        public async Task<IActionResult> UserExistsAction([FromQuery] string username)
        {
            if (await _context.Users.AnyAsync(x => x.Username.Equals(username.ToLower())) || username.Equals("users1"))
            {
                return Ok(new { Available = false });
            }
            else 
            {
                return Ok(new { Available = true });
            }
        }


        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username.Equals(username.ToLower()));
        }

        private UserDto GetUserDto(User user)
        {
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Token = _tokenService.CreateToken(user);

            return userDto;
        }
    }
}