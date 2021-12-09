using System.Net;

namespace api.Errors
{
    public class AuthenticationServiceNotFound : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public AuthenticationServiceNotFound(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "auth-0001";
            ErrorMessage = "Authentication Service not found";
            Detail = detail;
        }
    }
    
    public class BadToken : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public BadToken(string detail = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Error = "auth-0002";
            ErrorMessage = "Bad authentication token";
            Detail = detail;
        }
    }
    
    public class InvalidCredentials : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public InvalidCredentials(string detail = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Error = "auth-0003";
            ErrorMessage = "Invalid credentials";
            Detail = detail;
        }
    }
}