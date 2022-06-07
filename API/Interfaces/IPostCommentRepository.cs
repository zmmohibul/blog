using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPostCommentRepository
    {
        Task<Result<PostCommentDto>> CreateComment(int postId, CreatePostCommentDto postCommentDto, string username);
    }
}