using System;
using System.Linq;
using api.Contexts;
using api.Models.Organization;
using api.Models.User;
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
            var users = organization.Users.Select(user => _context.User.Local.Single(x => x.Id == user.Id)).ToList();
            organization.Users = users;
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

        public OrganizationModel[] GetOrganizations(int offset, int limit)
        {
            return _context.Organization.OrderBy(p => p.Id)
                .Skip(offset)
                .Take(limit)
                .ToArray();
        }

        public int CountOrganizations()
        {
            return _context.Organization.Count();
        }
    }
}