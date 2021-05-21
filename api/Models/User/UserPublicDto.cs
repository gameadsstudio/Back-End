using System;
using api.Enums.User;

namespace api.Models.User
{
    public class UserPublicDto : IUserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; }
    }
}