using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using api.Models.Organization;

namespace api.Models.User
{
    [Table("user")]
    public class UserModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Username { get; set; } // TODO: check id needed

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Alias { get; set; } // TODO: check if needed

        public string Phone { get; set; }

        public string Level { get; set; } // TODO: change to enum and rename to AdminLevel ?

        public string Status { get; set; } // TODO: what is it ? Need documentation

        public string DateStatus { get; set; } // TODO : Date as string ?

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }

        [JsonIgnore]
        [ForeignKey("OrgId")]
        public OrganizationModel Organization { get; set; }
        //public List<OrganizationModel> Users { get; set; }
        //public ICollection<OrganizationModel> Organizations { get; set; }
    }
}