using System.ComponentModel.DataAnnotations;
using api.DataAnnotations;
using api.Enums.User;

namespace api.Models.User
{
    public class UserCreationDto
    {
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(128)]
        public string Password { get; set; }

        [RequiredEnumAttribute]
        public UserType Type { get; set; }
    }
}