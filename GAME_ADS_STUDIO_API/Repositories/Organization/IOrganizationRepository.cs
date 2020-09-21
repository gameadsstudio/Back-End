using System.Collections.Generic;
using GAME_ADS_STUDIO_API.Models.Organization;

namespace GAME_ADS_STUDIO_API.Repositories.Organization
{
    public interface IOrganizationRepository
    {
        int AddNewOrganization(OrganizationModel organization);
        OrganizationModel GetOrganizationById(int id);
        int UpdateOrganization(OrganizationModel updatedOrganization, OrganizationModel targetOrganization);
        int DeleteOrganization(OrganizationModel organization);
    }
}
