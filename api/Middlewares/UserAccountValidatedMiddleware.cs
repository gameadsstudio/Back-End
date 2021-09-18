using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using api.Attributes;
using api.Helpers;
using api.Errors;

namespace api.Middlewares
{
    public class UserAccountValidatedMiddleware
    {
        private readonly RequestDelegate _next;

        private Endpoint _endpoint = null;

        private UserAccountValidatedAttribute _attribute = null;

        public UserAccountValidatedMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            ConnectedUser user = null;

            this._endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            this._attribute = this._endpoint
                ?.Metadata
                .GetMetadata<UserAccountValidatedAttribute>();
            if (this._attribute == null) {
                await this._next(context);
                return;
            }
            user = new ConnectedUser(context.User.Claims);
            if (!user.EmailValidated) {
                throw new ApiError(
                    HttpStatusCode.BadRequest,
                    "Email address not validated"
                );
            }
            await this._next(context);
        }
    }
}
