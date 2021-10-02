using System;
using System.Collections.Generic;
using api.Models.Organization;
using api.Models.User;
using api.Helpers;

namespace api.Business.Organization
{
    public interface IOrganizationBusinessLogic
    {
        OrganizationPrivateDto AddNewOrganization(OrganizationCreationDto newOrganization, ConnectedUser currentUser);
        object GetOrganizationById(string id, ConnectedUser currentUser);
        OrganizationModel GetOrganizationModelById(Guid id);
        public (int page, int pageSize, int totalItemCount, List<OrganizationPublicDto>) GetOrganizations(PagingDto paging, OrganizationFiltersDto filters);
        OrganizationPrivateDto UpdateOrganizationById(string id, OrganizationUpdateDto updatedOrganization, ConnectedUser currentUser);
        void DeleteOrganizationById(string id, ConnectedUser currentUser);
        OrganizationPrivateDto AddUserToOrganization(string id, string userId, ConnectedUser currentUser);
        List<UserPublicDto> GetOrganizationUsers(string id, ConnectedUser currentUser);
        OrganizationPrivateDto DeleteUserFromOrganization(string id, string userId, ConnectedUser currentUser);
        bool IsUserInOrganization(Guid orgId, Guid userId);
    }
}