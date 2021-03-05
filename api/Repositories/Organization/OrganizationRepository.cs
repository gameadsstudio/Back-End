using System;
using System.Linq;
using api.Contexts;
using api.Models.Organization;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Organization
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApiContext _context;
        private readonly DbSet<OrganizationModel> _repository;

        public OrganizationRepository(ApiContext context)
        {
            _context = context;
            _repository = _context.Organization;
        }

        public OrganizationModel AddNewOrganization(OrganizationModel organization)
        {
            _repository.Add(organization);
            return _context.SaveChanges() == 1 ? GetOrganizationById(organization.Id.ToString()) : null;
        }

        public int DeleteOrganization(OrganizationModel organization)
        {
            throw new NotImplementedException();
        }

        public OrganizationModel GetOrganizationById(string id)
        {
            return _repository.SingleOrDefault(e => e.Id.ToString() == id);
        }

        public int UpdateOrganization(OrganizationModel updatedOrganization, OrganizationModel targetOrganization)
        {
            throw new NotImplementedException();
        }
    }
}