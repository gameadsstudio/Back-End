using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models.User
{
    public class UserChangePasswordDto
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public Guid PasswordResetId { get; set; }
    }
}
