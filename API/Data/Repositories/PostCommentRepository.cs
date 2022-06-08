using System;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories
{
    public class PostCommentRepository : IPostCommentRepository
    {
        private readonly DataContext _context;
        public PostCommentRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<Result<PostCommentDto>> CreateComment(int postId, CreatePostCommentDto postCommentDto, string username)
        {
            var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == postId);
            if (post == null) 
            {
                return ReturnResults<PostCommentDto>.PostNotFoundResult();
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.Equals(username));


            if (user == null)
            {
                return ReturnResults<PostCommentDto>.UnauthorizedUserResult();
            }

            var postComment = new PostComment
            {
                Content = postCommentDto.Content,
                Post = post,
                User = user,
                CreatedAt = DateTime.Now
            };

            _context.PostComments.Add(postComment);
            await _context.SaveChangesAsync();

            return new Result<PostCommentDto>
            {
                IsSuccesful = true,
                StatusCode = 201,
                Data = GetPostCommentDto(postComment)
            };
        }

        private PostCommentDto GetPostCommentDto(PostComment postComment)
        {
            return new PostCommentDto
            {
                Id = postComment.Id,
                Content = postComment.Content,
                PostId = postComment.PostId,
                Username = postComment.User.Username,
                CreatedAt = postComment.CreatedAt
            };
        }
    }
}