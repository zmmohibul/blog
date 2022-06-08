using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Result<PostCommentToReturnDto>> GetComments(int postId, QueryParameters queryParameters)
        {
            var postComments = await _context.PostComments
                .OrderByDescending(c => c.CreatedAt)
                .Where(c => c.PostId == postId)
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .Include(c => c.User)
                .ToListAsync();

            var commentsCount = await _context.PostComments.CountAsync(c => c.PostId == postId);

            var commentsDto = new List<PostCommentDto>();
            foreach (var comment in postComments)
            {
                commentsDto.Add(GetPostCommentDto(postId, comment));
            }

            var commentsDtoToReturn = new PostCommentToReturnDto
            {
                Count = commentsCount,
                Comments = commentsDto
            };

            return new Result<PostCommentToReturnDto>
            {
                IsSuccesful = true,
                StatusCode = 201,
                Data = commentsDtoToReturn
            };
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
                Data = GetPostCommentDto(postId, postComment)
            };
        }

        private PostCommentDto GetPostCommentDto(int postId, PostComment postComment)
        {
            return new PostCommentDto
            {
                Id = postComment.Id,
                Content = postComment.Content,
                PostId = postId,
                Username = postComment.User.Username,
                CreatedAt = postComment.CreatedAt
            };
        }
    }
}