using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Organization;

namespace api.Models.User
{
    public class UserUpdateDto
    {
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(6)]
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [MinLength(8)]
        [MaxLength(128)]
        public string Password { get; set; }

        public string Alias { get; set; }

        public string Phone { get; set; }

        public string Level { get; set; }

        public string Status { get; set; }
    }
}