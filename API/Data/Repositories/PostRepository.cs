using System.Threading.Tasks;
using API.Dtos;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Result<PostToReturnDto>> CreatePostAsync(CreatePostDto createPostDto, string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.Equals(username));
            if (user == null)
            {
                return new Result<PostToReturnDto>
                {
                    IsSuccesful = false,
                    StatusCode = 401,
                    ErrorMessage = "Attempt to create post from unauthorized user"
                };
            }

            var post = _mapper.Map<Post>(createPostDto);
            post.CreatedBy = user;

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return new Result<PostToReturnDto>
            {
                IsSuccesful = true,
                StatusCode = 201,
                Data = _mapper.Map<PostToReturnDto>(post)
            };


            throw new System.NotImplementedException();
        }

        public async Task<Result<PostToReturnDto>> UpdatePostAsync(int postId, string username, UpdatePostDto updatePostDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username.Equals(username));

            var post = await _context.Posts
                .Include(p => p.CreatedBy)
                .SingleOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return new Result<PostToReturnDto>
                {
                    IsSuccesful = false,
                    StatusCode = 404,
                    ErrorMessage = "Post not found"
                };
            }
            System.Console.WriteLine(post.Id);
            System.Console.WriteLine(user.Username);
            System.Console.WriteLine(post.CreatedBy.Username);

            if (!post.CreatedBy.Username.Equals(username))
            {
                return new Result<PostToReturnDto>
                {
                    IsSuccesful = false,
                    StatusCode = 401,
                    ErrorMessage = "Unauthorized user"
                };
            }

            post.Content = updatePostDto.Content;
            post.Title = updatePostDto.Title;
            
            _context.Update(post);
            await _context.SaveChangesAsync();

            return new Result<PostToReturnDto>
            {
                IsSuccesful = true,
                StatusCode = 200,
                Data = _mapper.Map<PostToReturnDto>(post)
            };
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new System.NotImplementedException();
        }

        
    }
}