using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IPostRepository
    {
        Task<Post> CreatePostAsync(Post post, User createdByUser);
        Task<bool> SaveChangesAsync();
    }
}