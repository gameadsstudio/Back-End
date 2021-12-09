using System.Net;

namespace api.Errors
{
    public class CampaignInsufficientRightsError : ApiError
    {
        public CampaignInsufficientRightsError(string detail = "")
        {
            StatusCode = HttpStatusCode.Forbidden;
            Error = "campaign-0001";
            ErrorMessage = "Insufficient rights for action";
            Detail = detail;
        }
    }
    
    public class CampaignNotFoundError : ApiError
    {
        public CampaignNotFoundError(string detail = "")
        {
            StatusCode = HttpStatusCode.NotFound;
            Error = "campaign-0002";
            ErrorMessage = "Campaign not found";
            Detail = detail;
        }
    }
    
    public class CampaignStartAfterEndError : ApiError
    {
        public CampaignStartAfterEndError(string detail = "")
        {
            StatusCode = HttpStatusCode.BadRequest;
            Error = "campaign-0003";
            ErrorMessage = "Starting date cannot be set after end date";
            Detail = detail;
        }
    }
}