using api.Models.Organization;

namespace api.Repositories.Organization
{
    public interface IOrganizationRepository
    {
        int AddNewOrganization(OrganizationModel organization);
        OrganizationModel GetOrganizationById(int id);
        int UpdateOrganization(OrganizationModel updatedOrganization, OrganizationModel targetOrganization);
        int DeleteOrganization(OrganizationModel organization);
    }
}
