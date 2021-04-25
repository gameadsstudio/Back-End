using System.ComponentModel.DataAnnotations;

namespace api.Models.User
{
    public class UserLoginModel
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        public string Password { get; set; }
    }
}