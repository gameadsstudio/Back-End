﻿using System;
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
            var organization = _mapper.Map(newOrganization, new OrganizationModel());


            if (!(organization.Type == "Developers" || organization.Type == "Advertisers"))
            {
                throw new ApiError(HttpStatusCode.BadRequest, $"Organization type must be Developers or Advertisers");
            }

            if (_repository.GetOrganizationByName(organization.Name) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict, $"Organization with name: {organization.Name} already exists");
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

        public void DeleteOrganizationById(string id, Claim currentUser)
        {
            Guid guid;

            try
            {
                guid = Guid.Parse(id);
            }
            catch (Exception e)
            {
                throw new ApiError(HttpStatusCode.BadRequest, e.Message);
            }

            var organization = _repository.GetOrganizationById(guid);

            if (organization == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find organization with Id: {id}");
            }

            foreach (UserModel user in organization.Users)
            {
                if (user.Id.ToString() == currentUser.Value)
                {
                    _repository.DeleteOrganization(organization);
                }
            }
        }

        public IOrganizationModel GetOrganizationById(string id, Claim currentUser)
        {
            Guid guid;

            try
            {
                guid = Guid.Parse(id);
            }
            catch (Exception e)
            {
                throw new ApiError(HttpStatusCode.BadRequest, e.Message);
            }

            var organization = _repository.GetOrganizationById(guid);

            foreach (UserModel user in organization.Users)
            {
                if (user.Id.ToString() == currentUser.Value)
                {
                    return _mapper.Map(organization, new OrganizationPrivateModel());
                }
            }

            return _mapper.Map(organization, new OrganizationPublicModel());
        }

        public OrganizationPrivateModel UpdateOrganizationById(string id, OrganizationUpdateModel updatedOrganization, Claim currentUser)
        {
            Guid guid;

            try
            {
                guid = Guid.Parse(id);
            }
            catch (Exception e)
            {
                throw new ApiError(HttpStatusCode.BadRequest, e.Message);
            }

            var organization = _repository.GetOrganizationById(guid);

            if (organization == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find organization with Id: {id}");
            }

            foreach (UserModel user in organization.Users)
            {
                if (user.Id.ToString() == currentUser.Value)
                {
                    var organizationMapped = _mapper.Map(updatedOrganization, organization);

                    var result = _repository.UpdateOrganization(organizationMapped);

                    return _mapper.Map(result, new OrganizationPrivateModel());
                }
            }

            throw new ApiError(HttpStatusCode.NotModified, "Cannot modify organization");
        }

        public OrganizationModel AddUserToOrganization(string id, string userId, Claim currentUser)
        {
            Guid orgId;

            try
            {
                orgId = Guid.Parse(id);
            }
            catch (Exception e)
            {
                throw new ApiError(HttpStatusCode.BadRequest, e.Message);
            }

            var organization = _repository.GetOrganizationById(orgId);

            foreach (UserModel user in organization.Users)
            {
                if (user.Id.ToString() == currentUser.Value)
                {
                    Guid guid;

                    try
                    {
                        guid = Guid.Parse(userId);
                    }
                    catch (Exception e)
                    {
                        throw new ApiError(HttpStatusCode.BadRequest, e.Message);
                    }

                    var newUser = userRepository.GetUserById(guid);

                    organization.Users.Add(newUser);

                    return _repository.UpdateOrganization(organization);
                }
            }

            throw new ApiError(HttpStatusCode.NotModified, "Cannot add user to organization");
        }

        public List<UserModel> GetOrganizationUsers(string id, Claim currentUser)
        {
            Guid guid;

            try
            {
                guid = Guid.Parse(id);
            }
            catch (Exception e)
            {
                throw new ApiError(HttpStatusCode.BadRequest, e.Message);
            }

            var organization = _repository.GetOrganizationById(guid);

            foreach (UserModel user in organization.Users)
            {
                if (user.Id.ToString() == currentUser.Value)
                {
                    return organization.Users;
                }
            }

            return null;
        }
        public void DeleteUserFromOrganization(string id, string userId, Claim currentUser)
        {
            Guid orgId;

            try
            {
                orgId = Guid.Parse(id);
            }
            catch (Exception e)
            {
                throw new ApiError(HttpStatusCode.BadRequest, e.Message);
            }

            var organization = _repository.GetOrganizationById(orgId);

            if (organization == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find organization with Id: {id}");
            }

            foreach (UserModel user in organization.Users)
            {
                if (user.Id.ToString() == currentUser.Value)
                {
                    Guid guid;

                    try
                    {
                        guid = Guid.Parse(userId);
                    }
                    catch (Exception e)
                    {
                        throw new ApiError(HttpStatusCode.BadRequest, e.Message);
                    }

                    var newUser = userRepository.GetUserById(guid);

                    if (newUser == null)
                    {
                        throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find user with Id: {userId}");
                    }

                    organization.Users.Remove(newUser);

                    _repository.UpdateOrganization(organization);
                }
            }
        }
    }
}
