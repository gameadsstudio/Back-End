using System;
using Microsoft.AspNetCore.Http;

namespace api.Helpers
{
    public class UriHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _cdnUri;
        

        public UriHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _cdnUri = Environment.GetEnvironmentVariable("GAS_CDN_URI");
        }
        
        public Uri UriBuilder(string filename)
        {
            if (!string.IsNullOrEmpty(_cdnUri))
            {
                return new Uri($"https://{_cdnUri}{filename}");
            }

            return new Uri(
                $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host.Host}{filename}");
        }
    }
}