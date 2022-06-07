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
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt).Take(3))
                .AsNoTracking()
                .ToListAsync();

            var postToReturnDtoList = new List<PostToReturnDto>();
            foreach (var post in posts) 
            {
                postToReturnDtoList.Add(await GetPostToReturnDtoAsync(post));
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
                .Include(p => p.CreatedBy)
                .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt).Take(10))
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == postId);
            
            if (post == null) 
            {
                return ReturnResults<PostToReturnDto>.PostNotFoundResult();
            }

            return new Result<PostToReturnDto>
            {
                IsSuccesful = true,
                StatusCode = 200,
                Data = await GetPostToReturnDtoAsync(post)
            };
        }

        public async Task<Result<PostToReturnDto>> CreatePostAsync(CreatePostDto createPostDto, string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.Equals(username));
            if (user == null)
            {
                return ReturnResults<PostToReturnDto>.UnauthorizedUserResult();
            }

            var post = _mapper.Map<Post>(createPostDto);
            post.CreatedBy = user;
            post.CreatedAt = System.DateTime.Now;

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return await CreatedOrUpdatedPostResultAsync(post);
        }

        public async Task<Result<PostToReturnDto>> UpdatePostAsync(int postId, string username, UpdatePostDto updatePostDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.Equals(username));

            var post = await _context.Posts
                .Include(p => p.CreatedBy)
                .SingleOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return ReturnResults<PostToReturnDto>.PostNotFoundResult();
            }
            System.Console.WriteLine(post.Id);
            System.Console.WriteLine(user.Username);
            System.Console.WriteLine(post.CreatedBy.Username);

            if (!post.CreatedBy.Username.Equals(username))
            {
                return ReturnResults<PostToReturnDto>.UnauthorizedUserResult();
            }

            post.Content = updatePostDto.Content;
            post.Title = updatePostDto.Title;
            
            _context.Update(post);
            await _context.SaveChangesAsync();

            return await CreatedOrUpdatedPostResultAsync(post);
        }

        public async Task<Result<PostToReturnDto>> DeletePostAsync(int postId, string username)
        {
            var post = await _context.Posts
                .Include(p => p.CreatedBy)
                .SingleOrDefaultAsync(p => p.Id == postId);
            
            if (post == null) 
            {
                return ReturnResults<PostToReturnDto>.PostNotFoundResult();
            }

            if (!post.CreatedBy.Username.Equals(username))
            {
                return ReturnResults<PostToReturnDto>.UnauthorizedUserResult();
            }

            _context.Remove(post);
            await _context.SaveChangesAsync();

            return new Result<PostToReturnDto>
            {
                IsSuccesful = true,
                StatusCode = 204,
            };
        }

        private async Task<Result<PostToReturnDto>> CreatedOrUpdatedPostResultAsync(Post post)
        {
            return new Result<PostToReturnDto>
            {
                IsSuccesful = true,
                StatusCode = 201,
                Data = await GetPostToReturnDtoAsync(post)
            };
        }

        private async Task<PostToReturnDto> GetPostToReturnDtoAsync(Post post)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == post.CreatedBy.Id);
            var commentsDto = new List<PostCommentDto>();
            var commentCount = await _context.PostComments.CountAsync(c => c.PostId == post.Id);
            foreach (var comment in post.Comments) 
            {
                var commentDto = new PostCommentDto
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    PostId = comment.PostId,
                    Username = user.Username,
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
                Comments = commentsDto,
                NumberOfComments = commentCount
            };

            return postToReturnDto;
        }

    }
}