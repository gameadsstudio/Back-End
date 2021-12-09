using System.Net;

namespace api.Errors
{
    public class UsernameAlreadyExistError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public UsernameAlreadyExistError(string detail = "")
        {
            StatusCode = HttpStatusCode.Conflict;
            Error = "user-0001";
            ErrorMessage = "Username was already taken";
            Detail = detail;
        }
    }
    
    public class UserEmailAlreadyExistError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public UserEmailAlreadyExistError(string detail = "")
        {
            StatusCode = HttpStatusCode.Conflict;
            Error = "user-0002";
            ErrorMessage = "Email was already taken";
            Detail = detail;
        }
    }
    
    public class UserNotFoundError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public UserNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "user-0003";
            ErrorMessage = "User not found";
            Detail = detail;
        }
    }
    
    public class UserInvalidRightsError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public UserInvalidRightsError(string detail = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Error = "user-0004";
            ErrorMessage = "User has insufficient permissions";
            Detail = detail;
        }
    }
    
    public class AccountValidationError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public AccountValidationError(string detail = "")
        {
            StatusCode = HttpStatusCode.BadRequest;
            Error = "user-0005";
            ErrorMessage = "Account validation error";
            Detail = detail;
        }
    }
    
    public class ResetPasswordError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public ResetPasswordError(string detail = "")
        {
            StatusCode = HttpStatusCode.BadRequest;
            Error = "user-0006";
            ErrorMessage = "Error while resetting the password";
            Detail = detail;
        }
    }
    
    
}