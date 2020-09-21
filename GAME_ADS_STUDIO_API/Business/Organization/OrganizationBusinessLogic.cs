using GAME_ADS_STUDIO_API.Repositories.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GAME_ADS_STUDIO_API.Configuration;
using GAME_ADS_STUDIO_API.Contexts;
using Microsoft.Extensions.Options;
using GAME_ADS_STUDIO_API.Models.Organization;

namespace GAME_ADS_STUDIO_API.Business.Organization
{
    public class OrganizationBusinessLogic : IOrganizationBusinessLogic
    {
        private readonly IOrganizationRepository _repository;
        private readonly AppSettings _appSettings;

        public OrganizationBusinessLogic(IOptions<AppSettings> appSettings)
        {
            //_repository = new OrganizationRepository(context);
            _appSettings = appSettings.Value;
        }

        public OrganizationModel AddNewOrganization(OrganizationCreationModel newOrganization)
        {
            var organization = new OrganizationModel();

            organization.Name = newOrganization.Name;
            organization.PrivateEmail = newOrganization.PrivateEmail;
            organization.Type = newOrganization.Type;

            return organization;
        }

        public int DeleteOrganizationById(int id)
        {
            return 1;
        }

        public OrganizationModel GetOrganizationById(int id)
        {
            var organization = new OrganizationModel();

            organization.Id = (uint) id;
            organization.Name = "foobar";
            organization.Type = "Advertiser";

            return organization;
        }

        public int UpdateOrganizationById(int id, OrganizationUpdateModel updatedOrganization)
        {
            return 1;
        }
    }
}
