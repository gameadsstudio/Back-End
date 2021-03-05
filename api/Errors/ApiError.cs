using System.Net;

namespace api.Errors
{
    public class ApiError
    {
        public HttpStatusCode StatusCode;
        public string Message;

        public ApiError(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}