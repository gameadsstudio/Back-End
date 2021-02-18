using System;
using game_ads_studio_api.Contexts;
using game_ads_studio_api.Models.Organization;

namespace game_ads_studio_api.Repositories.Organization
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApiContext _context;

        public OrganizationRepository(ApiContext context)
        {
            _context = context;
        }

        public int AddNewOrganization(OrganizationModel organization)
        {
            throw new NotImplementedException();
        }

        public int DeleteOrganization(OrganizationModel organization)
        {
            throw new NotImplementedException();
        }

        public OrganizationModel GetOrganizationById(int id)
        {
            throw new NotImplementedException();
        }

        public int UpdateOrganization(OrganizationModel updatedOrganization, OrganizationModel targetOrganization)
        {
            throw new NotImplementedException();
        }
    }
}
