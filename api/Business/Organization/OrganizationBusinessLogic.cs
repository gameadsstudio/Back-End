using System;
using System.Collections.Generic;
using api.Configuration;
using api.Contexts;
using api.Models.Organization;
using api.Models.User;
using api.Repositories.Organization;
using Microsoft.Extensions.Options;

namespace api.Business.Organization
{
    public class OrganizationBusinessLogic : IOrganizationBusinessLogic
    {
        private readonly IOrganizationRepository _repository;
        private readonly AppSettings _appSettings;

        public OrganizationBusinessLogic(ApiContext context, IOptions<AppSettings> appSettings)
        {
            _repository = new OrganizationRepository(context);
            _appSettings = appSettings.Value;
        }

        public OrganizationModel AddNewOrganization(OrganizationCreationModel newOrganization)
        {
            var organization = new OrganizationModel
            {
                Name = newOrganization.Name,
                PrivateEmail = newOrganization.PrivateEmail,
                Type = newOrganization.Type
            };

            throw new NotImplementedException();
        }

        public int DeleteOrganizationById(string id)
        {
            throw new NotImplementedException();
        }

        public OrganizationModel GetOrganizationById(string id)
        {
            throw new NotImplementedException();
        }

        public OrganizationModel UpdateOrganizationById(string id, OrganizationUpdateModel updatedOrganization)
        {
            throw new NotImplementedException("");
        }

        // Todo : change return type
        public int AddUserToOrganization(string id, string userId)
        {
            throw new NotImplementedException();
        }
        
        public List<UserModel> GetOrganizationUsers(string id)
        {
            throw new NotImplementedException();
        }
        public int DeleteUserFromOrganization(string id, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
