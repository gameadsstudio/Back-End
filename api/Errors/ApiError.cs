using System;
using System.Net;

namespace api.Errors
{
    public class ApiError : Exception
    {
        public HttpStatusCode StatusCode;
        public new string Message;

        public ApiError(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}