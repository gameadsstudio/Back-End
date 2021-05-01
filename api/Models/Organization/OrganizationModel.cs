using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Campaign;
using api.Models.Game;
using api.Models.User;

namespace api.Models.Organization
{
    [Table("organization")]
    public class OrganizationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string PublicEmail { get; set; }

        [Required]
        public string PrivateEmail { get; set; }

        public string Localization { get; set; }

        public string LogoUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public string Type { get; set; }

        public string State { get; set; } // Todo : document

        public string DefaultAuthorization { get; set; } // Todo : document

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }

        public List<CampaignModel> Campaigns { get; set; }

        //public List<UserModel> Users { get; set; }

        public ICollection<UserModel> Users { get; set; }

        public List<GameModel> Games { get; set; }

    }
}
