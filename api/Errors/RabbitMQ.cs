using System.Net;

namespace api.Errors
{
    public class RabbitMQConnectionError : ApiError
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }

        public RabbitMQConnectionError(string detail = "")
        {
            StatusCode = HttpStatusCode.FailedDependency;
            Error = "rabbit-0001";
            ErrorMessage = "Cannot connect to message broker";
            Detail = detail;
        }
    }
}