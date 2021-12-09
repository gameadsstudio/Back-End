using System.Net;

namespace api.Errors
{
    public class RabbitMQConnectionError : ApiError
    {
        public RabbitMQConnectionError(string detail = "")
        {
            StatusCode = HttpStatusCode.FailedDependency;
            Error = "rabbit-0001";
            ErrorMessage = "Cannot connect to message broker";
            Detail = detail;
        }
    }
    
    public class RabbitSendPayloadError : ApiError
    {
        public RabbitSendPayloadError(string detail = "")
        {
            StatusCode = HttpStatusCode.FailedDependency;
            Error = "rabbit-0001";
            ErrorMessage = "Cannot connect to message broker";
            Detail = detail;
        }
    }
}