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
    
    public class BadTokenError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public BadTokenError(string detail = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Error = "auth-0002";
            ErrorMessage = "Bad authentication token";
            Detail = detail;
        }
    }
    
    public class InvalidCredentialsError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public InvalidCredentialsError(string detail = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Error = "auth-0003";
            ErrorMessage = "Invalid credentials";
            Detail = detail;
        }
    }
    
    public class AccountNotValidatedError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public AccountNotValidatedError(string detail = "")
        {
            StatusCode = HttpStatusCode.Unauthorized;
            Error = "auth-0004";
            ErrorMessage = "Account Validation Error";
            Detail = detail;
        }
    }
}