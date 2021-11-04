using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using api.Enums.User;
using api.Errors;

namespace api.Helpers
{
    public class ConnectedUser
    {
        public Guid Id { get; }

        public UserRole Role { get; }

        public bool EmailValidated { get; }

        public ConnectedUser(IEnumerable<Claim> claims)
        {
            Id = Guid.Parse(
                claims.FirstOrDefault(
                    p => p.Type == ClaimTypes.NameIdentifier
                )?.Value ?? throw new ApiError(
                    HttpStatusCode.BadRequest,
                    "Bad Token - No ID specified in token"
                )
            );
            Role = Enum.Parse<UserRole>(
                claims.FirstOrDefault(
                    p => p.Type == ClaimTypes.Role
                )?.Value ?? throw new ApiError(
                    HttpStatusCode.BadRequest,
                    "Bad Token - No role specified in token"
                )
            );
            EmailValidated = Boolean.Parse(
                claims.FirstOrDefault(
                    p => p.Type == "email_validated"
                )?.Value ?? "false"
            );
        }
    }
}
