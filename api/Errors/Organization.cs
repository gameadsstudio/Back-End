using System.Net;

namespace api.Errors
{
    public class OrganizationNameAlreadyExistError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public OrganizationNameAlreadyExistError(string detail = "")
        {
            StatusCode = HttpStatusCode.Conflict;
            Error = "org-0001";
            ErrorMessage = "Organization name already taken";
            Detail = detail;
        }
    }
    
    public class OrganizationEmailAlreadyExistError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public OrganizationEmailAlreadyExistError(string detail = "")
        {
            StatusCode = HttpStatusCode.Conflict;
            Error = "org-0002";
            ErrorMessage = "Organization email already taken";
            Detail = detail;
        }
    }
    
    public class OrganizationInsufficientRightsError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public OrganizationInsufficientRightsError(string detail = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Error = "org-0003";
            ErrorMessage = "Logged user cannot perform the action due to insuficient rights";
            Detail = detail;
        }
    }
    
    public class OrganizationNotFoundError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public OrganizationNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "org-0004";
            ErrorMessage = "Organization not found";
            Detail = detail;
        }
    }
    
    public class OrganizationUserAlreadyPresentError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public OrganizationUserAlreadyPresentError(string detail = "")
        {
            StatusCode = HttpStatusCode.Conflict;
            Error = "org-0005";
            ErrorMessage = "User already present in organization";
            Detail = detail;
        }
    }
    
    public class OrganizationUserNotFoundError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public OrganizationUserNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "org-0007";
            ErrorMessage = "User not found in organization";
            Detail = detail;
        }
    }
}