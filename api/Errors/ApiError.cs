using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace api.Errors
{
    public class ApiError : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string Detail { get; set; }
    }

}