using System;
using System.Threading.Tasks;
using API.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class PostCommentHub : Hub
    {
        private readonly PostCommentRepository _postCommentRepository;
        public PostCommentHub(PostCommentRepository postCommentRepository)
        {
            _postCommentRepository = postCommentRepository;

        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var postId = Convert.ToInt32(httpContext.Request.Query["postId"].ToString());
            
            

        }

    }
}