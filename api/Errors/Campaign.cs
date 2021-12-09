using System.Net;

namespace api.Errors
{
    public class CampaignInsufficientRightsError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

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
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

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
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public CampaignStartAfterEndError(string detail = "")
        {
            StatusCode = HttpStatusCode.BadRequest;
            Error = "campaign-0003";
            ErrorMessage = "Starting date cannot be set after end date";
            Detail = detail;
        }
    }
}