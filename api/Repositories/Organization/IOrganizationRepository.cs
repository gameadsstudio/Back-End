using api.Models.Organization;
using System;
using System.Collections.Generic;

namespace api.Repositories.Organization
{
    public interface IOrganizationRepository
    {
        OrganizationModel AddNewOrganization(OrganizationModel organization);
        OrganizationModel GetOrganizationById(Guid id);
        OrganizationModel GetOrganizationByName(string name);
        OrganizationModel GetOrganizationByEmail(string email);
        OrganizationModel UpdateOrganization(OrganizationModel updatedOrganization);
        void DeleteOrganization(OrganizationModel organization);
        List<OrganizationModel> GetOrganizations(OrganizationFiltersDto filters, int offset, int limit);
        int CountOrganizations(OrganizationFiltersDto filters);
    }
}
