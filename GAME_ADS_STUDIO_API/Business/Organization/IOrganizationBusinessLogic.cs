using GAME_ADS_STUDIO_API.Models.Organization;
using GAME_ADS_STUDIO_API.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAME_ADS_STUDIO_API.Business.Organization
{
    interface IOrganizationBusinessLogic
    {
        OrganizationModel AddNewOrganization(OrganizationCreationModel newOrganization);
        OrganizationModel GetOrganizationById(int id);
        int UpdateOrganizationById(int id, OrganizationUpdateModel updatedOrganization);
        int DeleteOrganizationById(int id);
        int AddUserToOrganization(int id, int userId);
        List<UserModel> GetOrganizationUsers(int id);
        int DeleteUserFromOrganization(int id, int userId);
    }
}