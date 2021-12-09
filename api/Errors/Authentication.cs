using System.Net;

namespace api.Errors
{
    public class AuthenticationServiceNotFound : ApiError
    {
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
        public AccountNotValidatedError(string detail = "")
        {
            StatusCode = HttpStatusCode.Unauthorized;
            Error = "auth-0004";
            ErrorMessage = "Account Validation Error";
            Detail = detail;
        }
    }
}