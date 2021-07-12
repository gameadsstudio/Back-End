using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
using api.Models.Organization;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Organization
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly ApiContext _context;

        public OrganizationRepository(ApiContext context)
        {
            _context = context;
        }

        public OrganizationModel AddNewOrganization(OrganizationModel organization)
        {
            _context.Organization.Add(organization);
            _context.SaveChanges();
            return organization;
        }

        public OrganizationModel GetOrganizationByName(string name)
        {
            return _context.Organization.SingleOrDefault(a => a.Name == name);
        }

        public OrganizationModel GetOrganizationByPrivateEmail(string email)
        {
            return _context.Organization.SingleOrDefault(a => a.PrivateEmail == email);
        }

        public void DeleteOrganization(OrganizationModel organization)
        {
            _context.Organization.Remove(organization);
            _context.SaveChanges();
        }

        public OrganizationModel GetOrganizationById(Guid id)
        {
            return _context.Organization.Include(p => p.Users).SingleOrDefault(e => e.Id == id);
        }

        public OrganizationModel UpdateOrganization(OrganizationModel updatedOrganization)
        {
            _context.Update(updatedOrganization);
            _context.SaveChanges();
            return updatedOrganization;
        }

        public List<OrganizationModel> GetOrganizations(OrganizationFiltersDto filters, int offset, int limit)
        {
            IQueryable<OrganizationModel> query = _context.Organization.OrderBy(a => a.Id);

            if (filters.UserId != null)
            {
                query = query.Where(o => o.Users.Any(u => u.Id == filters.UserId));
            }

            return query.Skip(offset).Take(limit).ToList();
        }

        public int CountOrganizations(OrganizationFiltersDto filters = null)
        {
            IQueryable<OrganizationModel> query = _context.Organization;

            if (filters?.UserId != null)
            {
                query = query.Where(o => o.Users.Any(u => u.Id == filters.UserId));
            }

            return query.Count();
        }
    }
}
