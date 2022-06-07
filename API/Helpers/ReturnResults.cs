using API.Dtos;

namespace API.Helpers
{
    public class ReturnResults<T>
    {
        public static Result<T> PostNotFoundResult()
        {
            return new Result<T>
            {
                IsSuccesful = false,
                StatusCode = 404,
                ErrorMessage = "Post not found"
            };
        }

        public static Result<T> UnauthorizedUserResult()
        {
            return new Result<T>
            {
                IsSuccesful = false,
                StatusCode = 401,
                ErrorMessage = "Unauthorized user"
            };
        }
    }
}