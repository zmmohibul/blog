using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IAuthRepository
    {
        Task<Result<UserDto>> RegisterUser(RegisterDto registerDto);
        
        Task<Result<UserDto>> UserLogin(LoginDto loginDto);

        Task<bool> UsernameExists(string username);

        Task<Result<UserDto>> GetUserByUsername(string username);

        Task<Result<List<UserDto>>> GetAllUsers();
    }
}