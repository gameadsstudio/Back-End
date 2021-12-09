using System.Net;

namespace api.Errors
{
    public class TagNotFoundError : ApiError
    {
        public TagNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "tag-0001";
            ErrorMessage = "Tag not found";
            Detail = detail;
        }
    }
    
    public class TagNameAlreadyExist : ApiError
    {
        public TagNameAlreadyExist(string detail = "")
        {
            StatusCode = HttpStatusCode.Conflict;
            Error = "tag-0002";
            ErrorMessage = "Tag name already exist";
            Detail = detail;
        }
    }
}