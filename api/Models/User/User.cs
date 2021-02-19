using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.User
{
    [Table("user")]
    public class UserModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Column("username")]
        public string Username { get; set; } // TODO: check id needed
        [Column("firstname")]
        public string Firstname { get; set; }
        [Column("lastname")]
        public string Lastname { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("alias")]
        public string Alias { get; set; } // TODO: check if needed
        [Column("phone")]
        public string Phone { get; set; }
        [Column("level")]
        public string Level { get; set; } // TODO: change to enum and rename to AdminLevel ?
        [Column("status")]
        public string Status { get; set; } // TODO: what is it ? Need documentation
        [Column("date_status")]
        public string DateStatus { get; set; } // TODO : Date as string ?
        [Column("date_creation")]
        public DateTimeOffset DateCreation { get; set; }
        [Column("date_update")]
        public DateTimeOffset DateUpdate { get; set; }
    }
}
