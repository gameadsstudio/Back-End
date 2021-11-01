using System.Net.Http;

namespace api.Helpers
{
    public class HttpHelper
    {
        private static readonly HttpClient httpClient;

        static HttpHelper()
        {
            httpClient = new HttpClient();
        }

        public static HttpClient getHttpInstance()
        {
            return httpClient;
        }
    }
}
