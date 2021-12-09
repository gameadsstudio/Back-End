using System.Net;

namespace api.Errors
{
    public class IncorrectCredentials : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public IncorrectCredentials(string detail)
        {
            this.StatusCode = HttpStatusCode.Forbidden;
            this.Error = "auth-0001";
            this.ErrorMessage = "";
            this.Detail = detail;
        }
    }
}