using api.Models.Organization;
using api.Models.User;

namespace api.Repositories.Organization
{
    public interface IOrganizationRepository
    {
        OrganizationModel AddNewOrganization(OrganizationModel organization);
        OrganizationModel GetOrganizationById(string id);
        OrganizationModel GetOrganizationByName(string name);
        OrganizationModel GetOrganizationByPublicEmail(string email);
        OrganizationModel GetOrganizationByPrivateEmail(string email);
        OrganizationModel UpdateOrganization(OrganizationModel updatedOrganization);
        int DeleteOrganization(OrganizationModel organization);
    }
}
