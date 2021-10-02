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
using api.Enums.User;
using AutoMapper;

namespace api.Business.Organization
{
    public class OrganizationBusinessLogic : IOrganizationBusinessLogic
    {
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _repository;
        private readonly IUserBusinessLogic _userBusinessLogic;

        public OrganizationBusinessLogic(ApiContext context, IMapper mapper, IUserBusinessLogic userBusinessLogic)
        {
            _repository = new OrganizationRepository(context);
            _userBusinessLogic = userBusinessLogic;
            _mapper = mapper;
        }

        public OrganizationPrivateDto AddNewOrganization(OrganizationCreationDto newOrganization,
            ConnectedUser currentUser)
        {
            var organization = _mapper.Map(newOrganization, new OrganizationModel());

            if (_repository.GetOrganizationByName(organization.Name) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"Organization with name: {organization.Name} already exists");
            }

            if (_repository.GetOrganizationByEmail(organization.Email) != null)
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"Organization with email: {organization.Email} already exists");
            }

            var user = _userBusinessLogic.GetUserModelById(currentUser.Id);

            organization.Users = new List<UserModel> {user};

            return _mapper.Map(_repository.AddNewOrganization(organization), new OrganizationPrivateDto());
        }

        public (int page, int pageSize, int totalItemCount, List<OrganizationPublicDto>) GetOrganizations(
            PagingDto paging, OrganizationFiltersDto filters)
        {
            paging = PagingHelper.Check(paging);
            var (organizations, totalItemCount) =
                _repository.GetOrganizations(filters, (paging.Page - 1) * paging.PageSize, paging.PageSize);
            return (paging.Page, paging.PageSize, totalItemCount,
                _mapper.Map(organizations, new List<OrganizationPublicDto>()));
        }

        public void DeleteOrganizationById(string id, ConnectedUser currentUser)
        {
            var organization = GetOrganizationModelById(GuidHelper.StringToGuidConverter(id));

            if (organization.Users != null && organization.Users.Any(x => x.Id == currentUser.Id))
            {
                _repository.DeleteOrganization(organization);
            }
            else
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot delete an organization which you are not a part of");
            }
        }

        // Todo : find a better return type
        public object GetOrganizationById(string id, ConnectedUser currentUser)
        {
            var organization = GetOrganizationModelById(GuidHelper.StringToGuidConverter(id));

            if (organization.Users != null && organization.Users.Any(user => user.Id == currentUser.Id) ||
                currentUser.Role == UserRole.Admin)
            {
                return _mapper.Map(organization, new OrganizationPrivateDto());
            }

            return _mapper.Map(organization, new OrganizationPublicDto());
        }

        public OrganizationModel GetOrganizationModelById(Guid id)
        {
            var organization = _repository.GetOrganizationById(id);

            if (organization == null)
            {
                throw new ApiError(HttpStatusCode.NotFound, $"Couldn't find organization with Id: {id}");
            }

            return organization;
        }

        public OrganizationPrivateDto UpdateOrganizationById(string id, OrganizationUpdateDto updatedOrganization,
            ConnectedUser currentUser)
        {
            var organization = GetOrganizationModelById(GuidHelper.StringToGuidConverter(id));

            if (organization.Users == null || organization.Users.All(user => user.Id != currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized,
                    "Cannot modify an organization which you are not a part of");
            }

            if (_repository.GetOrganizationByName(updatedOrganization.Name) != null &&
                updatedOrganization.Name != organization.Name)
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"Organization with name: {organization.Name} already exists");
            }

            if (_repository.GetOrganizationByEmail(updatedOrganization.Email) != null &&
                updatedOrganization.Email != organization.Email)
            {
                throw new ApiError(HttpStatusCode.Conflict,
                    $"Organization with email: {organization.Email} already exists");
            }

            var organizationMapped = _mapper.Map(updatedOrganization, organization);

            var result = _repository.UpdateOrganization(organizationMapped);

            return _mapper.Map(result, new OrganizationPrivateDto());
        }

        public OrganizationPrivateDto AddUserToOrganization(string id, string userId, ConnectedUser currentUser)
        {
            var organization = GetOrganizationModelById(GuidHelper.StringToGuidConverter(id));

            if (organization.Users == null || organization.Users.All(x => x.Id != currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Unauthorized,
                    "Cannot add user to an organization which you are not a part of");
            }

            if (organization.Users.Any(x => x.Id.ToString() == userId))
            {
                throw new ApiError(HttpStatusCode.Conflict, "User already in organization");
            }

            var user = _userBusinessLogic.GetUserModelById(new Guid(userId));

            organization.Users.Add(user);

            return _mapper.Map(_repository.UpdateOrganization(organization), new OrganizationPrivateDto());
        }

        public (int page, int pageSize, int totalItemCount, List<UserPublicDto> users) GetOrganizationUsers(string id,
            PagingDto paging, ConnectedUser currentUser)
        {
            var organization = GetOrganizationModelById(GuidHelper.StringToGuidConverter(id));

            var (page, pageSize, totalItemCount, users) = _userBusinessLogic.GetUsers(paging,
                new UserFiltersDto {OrganizationId = organization.Id}, currentUser);

            return (page, pageSize, totalItemCount, _mapper.Map(users, new List<UserPublicDto>()));
        }

        public OrganizationPrivateDto DeleteUserFromOrganization(string id, string userId, ConnectedUser currentUser)
        {
            var organization = GetOrganizationModelById(GuidHelper.StringToGuidConverter(id));

            if (organization.Users == null || organization.Users.All(x => x.Id != currentUser.Id))
            {
                throw new ApiError(HttpStatusCode.Forbidden,
                    "Cannot remove a user from an organization which you are not a part of");
            }

            var userToDelete = _userBusinessLogic.GetUserModelById(new Guid(userId));

            if (organization.Users.All(x => x.Id != userToDelete.Id))
            {
                throw new ApiError(HttpStatusCode.NotFound,
                    $"No user with Id: {userToDelete.Id} found in organization");
            }

            organization.Users.Remove(userToDelete);

            return _mapper.Map(_repository.UpdateOrganization(_mapper.Map(organization, new OrganizationModel())),
                new OrganizationPrivateDto());
        }

        public bool IsUserInOrganization(Guid orgId, Guid userId)
        {
            var org = GetOrganizationModelById(orgId);
            return org.Users?.Any(user => userId == user.Id) ?? false;
        }
    }
}