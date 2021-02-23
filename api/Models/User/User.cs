using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.User
{
    [Table("user")]
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Username { get; set; } // TODO: check id needed

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Alias { get; set; } // TODO: check if needed

        public string Phone { get; set; }

        public string Level { get; set; } // TODO: change to enum and rename to AdminLevel ?

        public string Status { get; set; } // TODO: what is it ? Need documentation

        public string DateStatus { get; set; } // TODO : Date as string ?

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}
