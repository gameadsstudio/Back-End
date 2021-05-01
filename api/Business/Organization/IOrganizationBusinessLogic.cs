using System.Collections.Generic;
using api.Models.Organization;
using api.Models.User;
using api.Helpers;
using System.Security.Claims;

namespace api.Business.Organization
{
    public interface IOrganizationBusinessLogic
    {
        OrganizationPrivateModel AddNewOrganization(OrganizationCreationModel newOrganization, Claim currentUser);
        IOrganizationModel GetOrganizationById(string id, Claim currentUser);
        public (int, int, int, OrganizationPublicModel[]) GetOrganizations(PagingDto paging);
        OrganizationPrivateModel UpdateOrganizationById(string id, OrganizationUpdateModel updatedOrganization, Claim currentUser);
        void DeleteOrganizationById(string id, Claim currentUser);
        OrganizationModel AddUserToOrganization(string id, string userId, Claim currentUser);
        ICollection<UserModel> GetOrganizationUsers(string id, Claim currentUser);
        void DeleteUserFromOrganization(string id, string userId, Claim currentUser);
    }
}