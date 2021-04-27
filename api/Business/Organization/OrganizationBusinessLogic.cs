using System;
using System.Collections.Generic;
using System.Net;
using api.Configuration;
using api.Contexts;
using api.Errors;
using api.Models.Organization;
using api.Models.User;
using api.Repositories.Organization;
using api.Repositories.User;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using AutoMapper;

namespace api.Business.Organization
{
    public class OrganizationBusinessLogic : IOrganizationBusinessLogic
    {
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _repository;
        private readonly IUserRepository userRepository;


        public OrganizationBusinessLogic(ApiContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _repository = new OrganizationRepository(context);
            userRepository = new UserRepository(context);
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        public OrganizationPrivateModel AddNewOrganization(OrganizationCreationModel newOrganization, Claim currentUser)
        {
            Console.Write("Coucou");
            var organization = _mapper.Map(newOrganization, new OrganizationModel());

            if (_repository.GetOrganizationByName(organization.Name) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict, $"Organization with name: {organization.Name} already exists");
            }

            if (_repository.GetOrganizationByPublicEmail(organization.PublicEmail) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict, $"Organization with public email: {organization.PublicEmail} already exists");
            }

            if (_repository.GetOrganizationByPrivateEmail(organization.PrivateEmail) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict, $"Organization with prvate email: {organization.PrivateEmail} already exists");
            }

            Guid guid;

            try
            {
                guid = Guid.Parse(currentUser.Value);
            }
            catch (Exception e)
            {
                throw new ApiError(HttpStatusCode.BadRequest, e.Message);
            }

            var user = userRepository.GetUserById(guid);

            organization.Users = new List<UserModel> { user };

            var result = _repository.AddNewOrganization(organization);

            return _mapper.Map(result, new OrganizationPrivateModel());
        }

        public int DeleteOrganizationById(string id, Claim currentUser)
        {
            var organization = _repository.GetOrganizationById(id);

            foreach (UserModel user in organization.Users)
            {
                if (user.Id.ToString() == currentUser.Value)
                {
                    
                    return _repository.DeleteOrganization(organization);
                }
            }
            return 2;
        }

        public IOrganizationModel GetOrganizationById(string id, Claim currentUser)
        {
            // Todo : add auth check

            var organization = _repository.GetOrganizationById(id);

            foreach (UserModel user in organization.Users)
            {
                if (user.Id.ToString() == currentUser.Value)
                {
                    return _mapper.Map(organization, new OrganizationPrivateModel());
                }
            }

            return _mapper.Map(organization, new OrganizationPublicModel());
        }

        public OrganizationModel UpdateOrganizationById(string id, OrganizationUpdateModel updatedOrganization)
        {
            throw new NotImplementedException("");
        }

        // Todo : change return type
        public int AddUserToOrganization(string id, string userId, Claim currentUser)
        {
            var organization = _repository.GetOrganizationById(id);

            foreach (UserModel user in organization.Users)
            {
                if (user.Id.ToString() == currentUser.Value)
                {
                    Guid guid;

                    try
                    {
                        guid = Guid.Parse(currentUser.Value);
                    }
                    catch (Exception e)
                    {
                        throw new ApiError(HttpStatusCode.BadRequest, e.Message);
                    }

                    var newUser = userRepository.GetUserById(guid);

                    organization.Users.Add(newUser);

                    _repository.UpdateOrganization(organization);

                    return 1;
                }
            }

            return 2;
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
