using System.Collections.Generic;
using api.Errors;
using api.Models.Organization;
using api.Models.User;

namespace api.Business.Organization
{
    internal interface IOrganizationBusinessLogic
    {
        (OrganizationModel, ApiError) AddNewOrganization(OrganizationCreationModel newOrganization);
        OrganizationModel GetOrganizationById(string id);
        OrganizationModel UpdateOrganizationById(string id, OrganizationUpdateModel updatedOrganization);
        int DeleteOrganizationById(string id);
        int AddUserToOrganization(string id, string userId);
        List<UserModel> GetOrganizationUsers(string id);
        int DeleteUserFromOrganization(string id, string userId);
    }
}