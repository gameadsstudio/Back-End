using System.Collections.Generic;
using api.Models.Organization;
using api.Models.User;
using api.Helpers;
using System.Security.Claims;

namespace api.Business.Organization
{
    public interface IOrganizationBusinessLogic
    {
        OrganizationPrivateDto AddNewOrganization(OrganizationCreationDto newOrganization, Claim currentUser);
        IOrganizationDto GetOrganizationById(string id, Claim currentUser);
        OrganizationModel GetOrganizationModelById(string id);
        public (int, int, int, List<OrganizationPublicDto>) GetOrganizations(PagingDto paging, OrganizationFiltersDto filters);
        OrganizationPrivateDto UpdateOrganizationById(string id, OrganizationUpdateDto updatedOrganization, Claim currentUser);
        void DeleteOrganizationById(string id, Claim currentUser);
        OrganizationPrivateDto AddUserToOrganization(string id, string userId, Claim currentUser);
        ICollection<UserPublicDto> GetOrganizationUsers(string id, Claim currentUser);
        OrganizationPrivateDto DeleteUserFromOrganization(string id, string userId, Claim currentUser);
    }
}