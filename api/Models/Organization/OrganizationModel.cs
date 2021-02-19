using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Organization
{
    [Table("organization")]
    public class OrganizationModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Column("public_email")]
        public string PublicEmail { get; set; }
        [Required]
        [Column("private_email")]
        public string PrivateEmail { get; set; }
        [Column("localization")]
        public string Localization { get; set; }
        [Column("logo_url")]
        public string LogoUrl { get; set; }
        [Column("website_url")]
        public string WebsiteUrl { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("state")]
        public string State { get; set; } // Todo : document
        [Column("default_authorization")]
        public string DefaultAuthorization { get; set; } // Todo : document
        [Column("creation_date")]
        public DateTimeOffset CreationDate { get; set; }
        [Column("modification_date")]
        public DateTimeOffset ModificationDate { get; set; }
    }
}
