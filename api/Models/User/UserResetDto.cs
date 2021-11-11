using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models.User
{
    public class UserResetDto
    {
        public Guid PasswordResetId { get; set; }

        [MinLength(8)]
        [MaxLength(128)]
        public string Password { get; set; }
    }
}
