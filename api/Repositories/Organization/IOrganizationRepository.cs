using api.Models.Organization;

namespace api.Repositories.Organization
{
    public interface IOrganizationRepository
    {
        OrganizationModel AddNewOrganization(OrganizationModel organization);
        OrganizationModel GetOrganizationById(string id);
        int UpdateOrganization(OrganizationModel updatedOrganization, OrganizationModel targetOrganization);
        int DeleteOrganization(OrganizationModel organization);
    }
}
