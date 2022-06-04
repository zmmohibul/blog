using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

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

        public async Task<Result<List<PostToReturnDto>>> GetAllPostsAsync()
        {
            var posts = await _context.Posts
                .ProjectTo<PostToReturnDto>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToListAsync();
            
            return new Result<List<PostToReturnDto>>
            {
                IsSuccesful = true,
                StatusCode = 200,
                Data = posts
            };
        }

        public async Task<Result<PostToReturnDto>> GetPostByIdAsync(int postId) 
        {
            var post = await _context.Posts
                .ProjectTo<PostToReturnDto>(_mapper.ConfigurationProvider)
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
                Data = post
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
                Data = _mapper.Map<PostToReturnDto>(post)
            };
        }

    }
}