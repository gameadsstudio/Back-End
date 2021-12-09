using System.Net;

namespace api.Errors
{
    public class PostNotFoundError : ApiError
    {
        public PostNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "post-0001";
            ErrorMessage = "Post not found";
            Detail = detail;
        }
    }
}