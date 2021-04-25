using System;
using System.Net;

namespace api.Errors
{
    public class ApiError : Exception
    {
        public new readonly string Message;
        public readonly HttpStatusCode StatusCode;

        public ApiError(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}