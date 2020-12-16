using GAME_ADS_STUDIO_API.Repositories.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GAME_ADS_STUDIO_API.Configuration;
using GAME_ADS_STUDIO_API.Contexts;
using Microsoft.Extensions.Options;
using GAME_ADS_STUDIO_API.Models.Organization;
using GAME_ADS_STUDIO_API.Models.User;

namespace GAME_ADS_STUDIO_API.Business.Organization
{
    public class OrganizationBusinessLogic : IOrganizationBusinessLogic
    {
        private readonly IOrganizationRepository _repository;
        private readonly AppSettings _appSettings;

        public OrganizationBusinessLogic(GasContext context, IOptions<AppSettings> appSettings)
        {
            _repository = new OrganizationRepository(context);
            _appSettings = appSettings.Value;
        }

        public OrganizationModel AddNewOrganization(OrganizationCreationModel newOrganization)
        {

            var organization = new OrganizationModel { org_name = newOrganization.org_name, org_email_private = newOrganization.org_email_private, org_type = newOrganization.org_type };

            return _repository.AddNewOrganization(organization) == 1 ? organization : null;
        }

        public OrganizationPublicModel[] GetOrganizations(int offset, int limit)
        {
            return _repository.GetOrganizations(offset, limit);
        }

        public OrganizationModel GetOrganizationById(int id)
        {
            if (id < 0)
                return null;

            var organization = _repository.GetOrganizationById(id);

            return organization;
        }

        public int UpdateOrganizationById(int id, OrganizationUpdateModel updatedOrganization)
        {
            var target = _repository.GetOrganizationById(id);

            if (target == null)
                return 2;
            if (updatedOrganization == null)
                return 0;

            if (updatedOrganization.media_id == null)
                updatedOrganization.media_id = target.media_id;

            if (updatedOrganization.org_name == null)
                updatedOrganization.org_name = target.org_name;

            if (updatedOrganization.org_email == null)
                updatedOrganization.org_email = target.org_email;

            if (updatedOrganization.org_email_private == null)
                updatedOrganization.org_email_private = target.org_email_private;

            if (updatedOrganization.org_city == null)
                updatedOrganization.org_city = target.org_city;

            if (updatedOrganization.org_address == null)
                updatedOrganization.org_address = target.org_address;

            if (updatedOrganization.org_url == null)
                updatedOrganization.org_url = target.org_url;

            if (updatedOrganization.org_type == null)
                updatedOrganization.org_type = target.org_type;

            if (updatedOrganization.org_status == null)
                updatedOrganization.org_status = target.org_status;

            if (updatedOrganization.org_level_default == null)
                updatedOrganization.org_level_default = target.org_level_default;

            return _repository.UpdateOrganization(updatedOrganization, target);
        }

        public int DeleteOrganizationById(int id)
        {
            if (id < 0)
                return 0;

            var organization = _repository.GetOrganizationById(id);

            if (organization == null)
                return 0;

            return _repository.DeleteOrganization(organization);
        }


        public int AddUserToOrganization(int id, int userId)
        {
            return 1;
        }
        public List<UserModel> GetOrganizationUsers(int id)
        {
            List<UserModel> users = new List<UserModel>();
            users.Add(new UserModel { Id = 1, Username = "JohnnyBoy", Firstname = "John", Lastname = "Doe" });
            users.Add(new UserModel { Id = 2, Username = "Jane", Firstname = "Jane", Lastname = "Doe" });
            return users;
        }
        public int DeleteUserFromOrganization(int id, int userId)
        {
            return 1;
        }
    }
}
