using System.Net;

namespace api.Errors
{
    public class AdvertisementInsufficientRightsError : ApiError
    {
        public AdvertisementInsufficientRightsError(string detail = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Error = "advertisement-0001";
            ErrorMessage = "Insufficient rights for action";
            Detail = detail;
        }
    }
    
    public class AdvertisementNotFoundError : ApiError
    {
        public AdvertisementNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "advertisement-0002";
            ErrorMessage = "Advertisement not found";
            Detail = detail;
        }
    }
}