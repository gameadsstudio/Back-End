using System.Net;

namespace api.Errors
{
    public class UsernameAlreadyExistError : ApiError
    {
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
        public ResetPasswordError(string detail = "")
        {
            StatusCode = HttpStatusCode.BadRequest;
            Error = "user-0006";
            ErrorMessage = "Error while resetting the password";
            Detail = detail;
        }
    }
    
    public class UserBadRequestError : ApiError
    {
        public UserBadRequestError(string detail = "")
        {
            StatusCode = HttpStatusCode.BadRequest;
            Error = "user-0007";
            ErrorMessage = "Bad request";
            Detail = detail;
        }
    }
}