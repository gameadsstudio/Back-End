﻿using System.ComponentModel.DataAnnotations;

namespace api.Models.User
{
    public class UserCreationDto
    {
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
    }
}