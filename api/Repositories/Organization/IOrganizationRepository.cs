using api.Models.Organization;
using System;

namespace api.Repositories.Organization
{
    public interface IOrganizationRepository
    {
        OrganizationModel AddNewOrganization(OrganizationModel organization);
        OrganizationModel GetOrganizationById(Guid id);
        OrganizationModel GetOrganizationByName(string name);
        OrganizationModel GetOrganizationByPrivateEmail(string email);
        OrganizationModel UpdateOrganization(OrganizationModel updatedOrganization);
        OrganizationModel DeleteOrganization(OrganizationModel organization);
        public OrganizationPublicModel[] GetOrganizations(int offset, int limit);
        public int CountOrganizations();
    }
}
