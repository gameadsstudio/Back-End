using System;

namespace api.Models.User
{
    public class UserFiltersDto
    {
        public string Username { get; set; }

        public string Email { get; set; }
        
        public Guid OrganizationId { get; set; }
    }
}