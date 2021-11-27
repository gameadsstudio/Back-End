using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Enums.Organization;
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

        public string StripeAccount { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string LogoUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public OrganizationType Type { get; set; }

        public long Money { get; set; }

        public ICollection<CampaignModel> Campaigns { get; set; }

        public ICollection<UserModel> Users { get; set; }

        public ICollection<GameModel> Games { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }
    }
}
