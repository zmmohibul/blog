using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> Register();
    }
}