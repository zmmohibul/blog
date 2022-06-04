using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;

namespace API.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _context;
        public PostRepository(DataContext context)
        {
            _context = context;

        }
        public Task<Post> CreatePostAsync(Post post, User createdByUser)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}