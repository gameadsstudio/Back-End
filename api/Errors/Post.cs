using System.Net;

namespace api.Errors
{
    public class PostNotFoundError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public PostNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "post-0001";
            ErrorMessage = "Post not found";
            Detail = detail;
        }
    }
}