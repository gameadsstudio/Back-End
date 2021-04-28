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
        OrganizationPrivateModel UpdateOrganizationById(string id, OrganizationUpdateModel updatedOrganization, Claim currentUser);
        OrganizationModel DeleteOrganizationById(string id, Claim currentUser);
        OrganizationModel AddUserToOrganization(string id, string userId, Claim currentUser);
        List<UserModel> GetOrganizationUsers(string id, Claim currentUser);
        OrganizationModel DeleteUserFromOrganization(string id, string userId, Claim currentUser);
    }
}