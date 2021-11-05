using System;
using api.Enums.User;

namespace api.Models.User
{
    public class UserPrivateDto : IUserDto
    {
        public Guid Id { get; set; }

        public UserRole Role { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        
        public Uri ProfilePictureUrl { get; set; }

        public Boolean EmailValidated { get; set; }

        public UserType Type { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}
