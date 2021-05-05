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
        void DeleteOrganization(OrganizationModel organization);
        public OrganizationModel[] GetOrganizations(int offset, int limit);
        public int CountOrganizations();
    }
}
