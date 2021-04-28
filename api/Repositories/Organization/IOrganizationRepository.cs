using api.Models.Organization;
using api.Models.User;

namespace api.Repositories.Organization
{
    public interface IOrganizationRepository
    {
        OrganizationModel AddNewOrganization(OrganizationModel organization);
        OrganizationModel GetOrganizationById(string id);
        OrganizationModel GetOrganizationByName(string name);
        OrganizationModel GetOrganizationByPrivateEmail(string email);
        OrganizationModel UpdateOrganization(OrganizationModel updatedOrganization);
        OrganizationModel DeleteOrganization(OrganizationModel organization);
    }
}
