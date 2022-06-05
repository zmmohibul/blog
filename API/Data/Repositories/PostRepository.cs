using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using System.Linq;

namespace API.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PostRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }

        public async Task<Result<PagedResult<PostToReturnDto>>> GetAllPostsAsync(QueryParameters queryParameters)
        {   
            var posts = await _context.Posts
                .Include(p => p.CreatedBy)
                .OrderByDescending(p => p.CreatedAt)
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt).Take(5))
                .AsNoTracking()
                .ToListAsync();

            var postToReturnDtoList = new List<PostToReturnDto>();
            foreach (var post in posts) 
            {
                postToReturnDtoList.Add(GetPostToReturnDto(post));
            }

            return new Result<PagedResult<PostToReturnDto>>
            {
                IsSuccesful = true,
                StatusCode = 200,
                Data = new PagedResult<PostToReturnDto>
                {
                    Count = await _context.Posts.CountAsync(),
                    PageSize = queryParameters.PageSize,
                    PageNumber = queryParameters.PageNumber,
                    Data = postToReturnDtoList
                }
            };
        }

        public async Task<Result<PostToReturnDto>> GetPostByIdAsync(int postId) 
        {
            var post = await _context.Posts
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt).Take(10))
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == postId);
            
            if (post == null) 
            {
                return PostNotFoundResult();
            }

            return new Result<PostToReturnDto>
            {
                IsSuccesful = true,
                StatusCode = 200,
                Data = GetPostToReturnDto(post)
            };
        }

        public async Task<Result<PostToReturnDto>> CreatePostAsync(CreatePostDto createPostDto, string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.Equals(username));
            if (user == null)
            {
                return UnauthorizedUserResult();
            }

            var post = _mapper.Map<Post>(createPostDto);
            post.CreatedBy = user;
            post.CreatedAt = System.DateTime.Now;

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedOrUpdatedPostResult(post);
        }

        public async Task<Result<PostToReturnDto>> UpdatePostAsync(int postId, string username, UpdatePostDto updatePostDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.Equals(username));

            var post = await _context.Posts
                .Include(p => p.CreatedBy)
                .SingleOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return PostNotFoundResult();
            }
            System.Console.WriteLine(post.Id);
            System.Console.WriteLine(user.Username);
            System.Console.WriteLine(post.CreatedBy.Username);

            if (!post.CreatedBy.Username.Equals(username))
            {
                return UnauthorizedUserResult();
            }

            post.Content = updatePostDto.Content;
            post.Title = updatePostDto.Title;
            
            _context.Update(post);
            await _context.SaveChangesAsync();

            return CreatedOrUpdatedPostResult(post);
        }

        public async Task<Result<PostToReturnDto>> DeletePostAsync(int postId, string username)
        {
            var post = await _context.Posts
                .Include(p => p.CreatedBy)
                .SingleOrDefaultAsync(p => p.Id == postId);
            
            if (post == null) 
            {
                return PostNotFoundResult();
            }

            if (!post.CreatedBy.Username.Equals(username))
            {
                return UnauthorizedUserResult();
            }

            _context.Remove(post);
            await _context.SaveChangesAsync();

            return new Result<PostToReturnDto>
            {
                IsSuccesful = true,
                StatusCode = 204,
            };
        }


        private Result<PostToReturnDto> PostNotFoundResult()
        {
            return new Result<PostToReturnDto>
            {
                IsSuccesful = false,
                StatusCode = 404,
                ErrorMessage = "Post not found"
            };
        }

        private Result<PostToReturnDto> UnauthorizedUserResult()
        {
            return new Result<PostToReturnDto>
            {
                IsSuccesful = false,
                StatusCode = 401,
                ErrorMessage = "Unauthorized user"
            };
        }

        private Result<PostToReturnDto> CreatedOrUpdatedPostResult(Post post)
        {
            return new Result<PostToReturnDto>
            {
                IsSuccesful = true,
                StatusCode = 201,
                Data = GetPostToReturnDto(post)
            };
        }

        private PostToReturnDto GetPostToReturnDto(Post post)
        {
            var commentsDto = new List<PostCommentDto>();
            foreach (var comment in post.Comments) 
            {
                var commentDto = new PostCommentDto
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    PostId = comment.PostId,
                    UserId = comment.UserId,
                    CreatedAt = comment.CreatedAt
                };
                commentsDto.Add(commentDto);
            }

            var postToReturnDto = new PostToReturnDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                CreatedBy = post.CreatedBy.Username,
                Comments = commentsDto
            };

            return postToReturnDto;
        }

    }
}