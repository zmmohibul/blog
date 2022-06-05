using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public AuthRepository(DataContext context, IMapper mapper, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _context = context;

        }

        public async Task<Result<UserDto>> RegisterUser(RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username.Equals(registerDto.Username)))
            {
                return new Result<UserDto>
                {
                    StatusCode = 400,
                    ErrorMessage = "Username is taken",
                    IsSuccesful = false,
                    Data = null
                };
            }

            var user = _mapper.Map<User>(registerDto);

            using var hmac = new HMACSHA512();

            user.Username = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            user.CreatedAt = System.DateTime.Now;
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = new Result<UserDto>
            {
                IsSuccesful = true,
                StatusCode = 200,
                Data = _mapper.Map<UserDto>(user)
            };

            result.Data.Token = _tokenService.CreateToken(user);

            return result;
        }

        public async Task<Result<UserDto>> UserLogin(LoginDto loginDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(x => x.Username.Equals(loginDto.Username));

            if (user == null)
            {
                return new Result<UserDto>
                {
                    StatusCode = 401,
                    ErrorMessage = "Invalid Username",
                    IsSuccesful = false,
                    Data = null
                };
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return new Result<UserDto>
                    {
                        StatusCode = 401,
                        ErrorMessage = "Invalid Password",
                        IsSuccesful = false,
                        Data = null
                    };
                }
            }

            var result = new Result<UserDto>
            {
                IsSuccesful = true,
                StatusCode = 200,
                Data = _mapper.Map<UserDto>(user)
            };

            result.Data.Token = _tokenService.CreateToken(user);

            return result;
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username.Equals(username));
        }

        public async Task<Result<List<UserDto>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return new Result<List<UserDto>>
            {
                IsSuccesful = true,
                StatusCode = 200,
                Data = _mapper.Map<List<UserDto>>(users)
            };
        }

        public async Task<Result<UserDto>> GetUserByUsername(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.Equals(username));
            if (user == null)
            {
                return new Result<UserDto>
                {
                    StatusCode = 404,
                    ErrorMessage = "User with that username does not exist",
                    Data = null,
                    IsSuccesful = false
                };
            }

            

            return new Result<UserDto>
            {
                StatusCode = 201,
                IsSuccesful = true,
                Data = _mapper.Map<UserDto>(user)
            };
        }
    }
}