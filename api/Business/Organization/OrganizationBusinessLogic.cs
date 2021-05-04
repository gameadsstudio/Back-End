using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using api.Contexts;
using api.Errors;
using api.Models.Organization;
using api.Models.User;
using api.Helpers;
using api.Repositories.Organization;
using api.Business.User;
using System.Security.Claims;
using System.Text.Json;
using AutoMapper;

namespace api.Business.Organization
{
    public class OrganizationBusinessLogic : IOrganizationBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _repository;
        private readonly IUserBusinessLogic _userBusinessLogic;

        public OrganizationBusinessLogic(ApiContext context, IMapper mapper,
            IUserBusinessLogic userBusinessLogic)
        {
            _repository = new OrganizationRepository(context);
            _userBusinessLogic = userBusinessLogic;
            _mapper = mapper;
        }

        public OrganizationPrivateDto AddNewOrganization(OrganizationCreationDto newOrganization, Claim currentUser)
        {
            var organization = _mapper.Map(newOrganization, new OrganizationModel());

            if (!(organization.Type == "Developers" || organization.Type == "Advertisers"))
            {
                throw new ApiError(HttpStatusCode.BadRequest, $"Organization type must be Developers or Advertisers");
            }

            if (_repository.GetOrganizationByName(organization.Name) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"Organization with name: {organization.Name} already exists");
            }

            if (_repository.GetOrganizationByPrivateEmail(organization.PrivateEmail) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"Organization with private email: {organization.PrivateEmail} already exists");
            }

            var user = _userBusinessLogic.GetUserById(currentUser.Value, currentUser);

            organization.Users = new List<UserModel> {_mapper.Map(user, new UserModel())};

            return _mapper.Map(_repository.AddNewOrganization(organization), new OrganizationPrivateDto());
        }

        public (int, int, int, OrganizationPublicDto[]) GetOrganizations(PagingDto paging)
        {
            paging = PagingHelper.Check(paging);
            var maxPage = _repository.CountOrganizations() / paging.PageSize + 1;
            var organizations = _repository.GetOrganizations((paging.Page - 1) * paging.PageSize, paging.PageSize);
            return (paging.Page, paging.PageSize, maxPage, organizations);
        }

        public void DeleteOrganizationById(string id, Claim currentUser)
        {
            var organization = _repository.GetOrganizationById(GuidHelper.StringToGuidConverter(id));

            if (organization == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find organization with Id: {id}");
            }

            if (organization.Users.Any(x => x.Id.ToString() == currentUser.Value))
            {
                _repository.DeleteOrganization(organization);
            }
        }

        public IOrganizationDto GetOrganizationById(string id, Claim currentUser)
        {
            var organization = _repository.GetOrganizationById(GuidHelper.StringToGuidConverter(id));

            if (organization == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find organization with Id: {id}");
            }
            
            if (organization.Users.Any(user => user.Id.ToString() == currentUser.Value))
            {
                return _mapper.Map(organization, new OrganizationPrivateDto());
            }

            return _mapper.Map(organization, new OrganizationPublicDto());
        }

        public OrganizationPrivateDto UpdateOrganizationById(string id, OrganizationUpdateDto updatedOrganization,
            Claim currentUser)
        {
            var organization = _repository.GetOrganizationById(GuidHelper.StringToGuidConverter(id));

            if (organization == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find organization with Id: {id}");
            }

            if (organization.Users.All(user => user.Id.ToString() != currentUser.Value))
            {
                throw new ApiError(HttpStatusCode.NotModified, "Cannot modify organization");
            }

            var organizationMapped = _mapper.Map(updatedOrganization, organization);

            organizationMapped.DateUpdate = DateTime.UtcNow;

            var result = _repository.UpdateOrganization(organizationMapped);

            return _mapper.Map(result, new OrganizationPrivateDto());
        }

        public OrganizationPrivateDto AddUserToOrganization(string id, string userId, Claim currentUser)
        {
            var organization = _repository.GetOrganizationById(GuidHelper.StringToGuidConverter(id));

            if (organization.Users.All(user => user.Id.ToString() != currentUser.Value))
            {
                throw new ApiError(HttpStatusCode.NotModified, "Cannot add user to organization");
            }

            if (organization.Users.Any(x => x.Id.ToString() == userId))
            {
                throw new ApiError(HttpStatusCode.Conflict, "User already in organization");
            }

            var newUser = _userBusinessLogic.GetUserById(userId, currentUser);

            organization.Users.Add(_mapper.Map(newUser, new UserModel()));

            return _mapper.Map(_repository.UpdateOrganization(organization), new OrganizationPrivateDto());
        }

        public ICollection<UserPublicDto> GetOrganizationUsers(string id, Claim currentUser)
        {
            var organization = _repository.GetOrganizationById(GuidHelper.StringToGuidConverter(id));

            if (organization == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find organization with Id: {id}");
            }
            
            if (organization.Users != null && organization.Users.Any(x => x.Id.ToString() == currentUser.Value))
            {
                throw new ApiError(HttpStatusCode.Forbidden, "User does not belong in the organization");
            }

            return _mapper.Map(organization.Users, new List<UserPublicDto>());
        }

        public void DeleteUserFromOrganization(string id, string userId, Claim currentUser)
        {
            var organization = _repository.GetOrganizationById(GuidHelper.StringToGuidConverter(id));

            if (organization == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find organization with Id: {id}");
            }

            if (organization.Users.All(x => x.Id.ToString() != currentUser.Value)) return;

            var userToDelete = _userBusinessLogic.GetUserById(userId, currentUser);

            if (userToDelete == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find user with Id: {userId}");
            }

            organization.Users.Remove(_mapper.Map(userToDelete, new UserModel()));

            _repository.UpdateOrganization(organization);
        }
    }
}