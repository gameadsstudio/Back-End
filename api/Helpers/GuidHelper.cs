using System;
using System.Net;
using api.Errors;

namespace api.Helpers
{
    public static class GuidHelper
    {
        public static Guid StringToGuidConverter(string id)
        {
            try
            {
                return Guid.Parse(id);
            }
            catch (Exception e)
            {
                throw new ApiError(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}