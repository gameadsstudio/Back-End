using game_ads_studio_api.Models.Organization;

namespace game_ads_studio_api.Repositories.Organization
{
    public interface IOrganizationRepository
    {
        int AddNewOrganization(OrganizationModel organization);
        OrganizationModel GetOrganizationById(int id);
        int UpdateOrganization(OrganizationModel updatedOrganization, OrganizationModel targetOrganization);
        int DeleteOrganization(OrganizationModel organization);
    }
}
