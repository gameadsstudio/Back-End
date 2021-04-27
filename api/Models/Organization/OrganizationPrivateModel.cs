using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Campaign;
using api.Models.Game;
using api.Models.User;

namespace api.Models.Organization
{
    public class OrganizationPrivateModel : IOrganizationModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PublicEmail { get; set; }

        public string PrivateEmail { get; set; }

        public string Localization { get; set; }

        public string LogoUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public string Type { get; set; }

        public string State { get; set; }

        public string DefaultAuthorization { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset ModificationDate { get; set; }

        public List<CampaignModel> Campaigns { get; set; }

        public List<UserModel> Users { get; set; }

        public List<GameModel> Games { get; set; }
    }
}
