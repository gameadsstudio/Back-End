using System.Net;

namespace api.Errors
{
    public class Version
    {
        public class VersionInvalidRightsError : ApiError
        {
            public VersionInvalidRightsError(string detail = "")
            {
                StatusCode = HttpStatusCode.Forbidden;
                Error = "vers-0001";
                ErrorMessage = "Insufficient rights";
                Detail = detail;
            }
        }
        
        public class VersionSemVerAlreadyExistError : ApiError
        {
            public VersionSemVerAlreadyExistError(string detail = "")
            {
                StatusCode = HttpStatusCode.Conflict;
                Error = "vers-0002";
                ErrorMessage = "Version with SemVer already exist";
                Detail = detail;
            }
        }
        
        public class VersionNotFoundError : ApiError
        {
            public VersionNotFoundError(string detail = "")
            {
                StatusCode = HttpStatusCode.NotFound;
                Error = "vers-0003";
                ErrorMessage = "Version not found";
                Detail = detail;
            }
        }
        
    }
}