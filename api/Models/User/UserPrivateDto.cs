using System;

namespace api.Models.User
{
    public class UserPrivateDto : IUserDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Alias { get; set; }

        public string Phone { get; set; }

        public string Level { get; set; }

        public string Status { get; set; }

        public string DateStatus { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}