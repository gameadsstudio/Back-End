using System;
using System.Collections.Generic;
using api.Enums.Organization;
using api.Models.Campaign;
using api.Models.Game;
using api.Models.User;

namespace api.Models.Organization
{
    public class OrganizationPrivateDto : IOrganizationDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PublicEmail { get; set; }

        public string PrivateEmail { get; set; }

        public string Localization { get; set; }

        public string LogoUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public OrganizationType Type { get; set; }

        public OrganizationState State { get; set; }

        public OrganizationUserAuthorization DefaultAuthorization { get; set; }

        public DateTimeOffset DateCreation { get; set; }

        public DateTimeOffset DateUpdate { get; set; }

        public List<CampaignModel> Campaigns { get; set; }

        public List<UserPublicDto> Users { get; set; }

        public List<GameModel> Games { get; set; }
    }
}
