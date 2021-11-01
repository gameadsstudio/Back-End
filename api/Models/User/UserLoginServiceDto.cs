using System.ComponentModel.DataAnnotations;
using api.Enums.Auth;

namespace api.Models.User
{
    public class UserLoginServiceDto
    {
        [Required]
        public Providers Provider { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
