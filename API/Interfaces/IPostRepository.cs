using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPostRepository
    {
        Task<Result<PostToReturnDto>> CreatePostAsync(CreatePostDto createPostDto, string username);

        Task<Result<PostToReturnDto>> UpdatePostAsync(int postId, string username, UpdatePostDto updatePostDto);
        Task<bool> SaveChangesAsync();
    }
}