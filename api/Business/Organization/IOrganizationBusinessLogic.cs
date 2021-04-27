using System.Collections.Generic;
using api.Models.Organization;
using api.Models.User;
using System.Security.Claims;

namespace api.Business.Organization
{
    public interface IOrganizationBusinessLogic
    {
        OrganizationPrivateModel AddNewOrganization(OrganizationCreationModel newOrganization, Claim currentUser);
        IOrganizationModel GetOrganizationById(string id, Claim currentUser);
        OrganizationModel UpdateOrganizationById(string id, OrganizationUpdateModel updatedOrganization);
        int DeleteOrganizationById(string id, Claim currentUser);
        int AddUserToOrganization(string id, string userId, Claim currentUser);
        List<UserModel> GetOrganizationUsers(string id);
        int DeleteUserFromOrganization(string id, string userId);
    }
}