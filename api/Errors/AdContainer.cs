using System;
using System.Net;

namespace api.Errors
{
    public class AdContainerInsufficientRightsError : ApiError
    {
        public AdContainerInsufficientRightsError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "adContainer-0001";
            ErrorMessage = "Insufficient rights for action";
            Detail = detail;
        }
    }
    
    public class AdContainerNotFoundError : ApiError
    {
        public AdContainerNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "adContainer-0002";
            ErrorMessage = "AdContainer not found";
            Detail = detail;
        }
    }
}