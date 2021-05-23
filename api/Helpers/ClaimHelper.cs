using System;
using System.Linq;
using System.Security.Claims;
using api.Enums;
using api.Enums.User;

namespace api.Helpers
{
    public class CurrentUser
    {
        public Guid? Id { get; }
        public UserRole? Role { get; }

        public CurrentUser(System.Collections.Generic.IEnumerable<Claim> claims)
        {
            var tmp1 = claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
            if (tmp1 != null)
            {
                Id = Guid.Parse(tmp1.Value);
            }
            else
            {
                Id = null;
            }
            
            var tmp2 = claims.FirstOrDefault(p => p.Type == ClaimTypes.Role);
            if (tmp2 != null)
            {
                Role = Enum.Parse<UserRole>(tmp2.Value);
            }
            else
            {
                Id = null;
            }
        }
    }
    
    public static class ClaimHelper
    {
        // public static CurrentUser ClaimToUser(System.Collections.Generic.IEnumerable<Claim> claims)
        // {
        //     var currentUser = new CurrentUser();
        //     
        //     var id = claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);
        //     var role = claims.FirstOrDefault(p => p.Type == ClaimTypes.Role);
        //     
        //     Console.WriteLine(claims);
        //     return currentUser;
        // }
    }
}