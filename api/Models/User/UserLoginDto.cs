using System.ComponentModel.DataAnnotations;

namespace api.Models.User
{
    public class UserLoginDto
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        public string Password { get; set; }
    }
}