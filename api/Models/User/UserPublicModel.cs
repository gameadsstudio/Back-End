using System;

namespace api.Models.User
{
    public class UserPublicModel : IUserModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}