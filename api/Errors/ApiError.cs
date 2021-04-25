using System;
using System.Net;

namespace api.Errors
{
    public class ApiError : Exception
    {
        public new string Message;
        public HttpStatusCode StatusCode;

        public ApiError(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}