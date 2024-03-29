﻿using System.ComponentModel.DataAnnotations;
using api.DataAnnotations;
using api.Enums.User;
using Microsoft.AspNetCore.Http;

namespace api.Models.User
{
    public class UserCreationDto
    {
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
        
        public IFormFile ProfilePicture { get; set; }
    }
}