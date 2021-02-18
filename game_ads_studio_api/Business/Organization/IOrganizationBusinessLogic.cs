using System.Collections.Generic;
using game_ads_studio_api.Models.Organization;
using game_ads_studio_api.Models.User;

namespace game_ads_studio_api.Business.Organization
{
    internal interface IOrganizationBusinessLogic
    {
        OrganizationModel AddNewOrganization(OrganizationCreationModel newOrganization);
        OrganizationModel GetOrganizationById(string id);
        OrganizationModel UpdateOrganizationById(string id, OrganizationUpdateModel updatedOrganization);
        int DeleteOrganizationById(string id);
        int AddUserToOrganization(string id, string userId);
        List<UserModel> GetOrganizationUsers(string id);
        int DeleteUserFromOrganization(string id, string userId);
    }
}