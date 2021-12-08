using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
using api.Models.AdContainer;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.AdContainer
{
    public class AdContainerRepository : IAdContainerRepository
    {
        private readonly ApiContext _context;

        public AdContainerRepository(ApiContext context)
        {
            _context = context;
        }

        public AdContainerModel AddNewAdContainer(AdContainerModel adContainer)
        {
            _context.AdContainer.Add(adContainer);
            _context.SaveChanges();
            return adContainer;
        }

        public AdContainerModel GetAdContainerById(Guid id)
        {
            return _context.AdContainer.Include(a => a.Version)
                .Include(a => a.Tags)
                .Include(a => a.Organization)
                .SingleOrDefault(a => a.Id == id);
        }

        public AdContainerModel GetAdContainerByName(string name)
        {
            return _context.AdContainer.Include(a => a.Tags)
                .Include(a => a.Organization)
                .Include(a => a.Version)
                .SingleOrDefault(a => a.Name == name);
        }

        public (List<AdContainerModel>, int totalItemCount) GetAdContainers(int offset, int limit,
            AdContainerFiltersDto filters, Guid userId)
        {
            IQueryable<AdContainerModel> query = _context.AdContainer.OrderBy(a => a.DateCreation);

            query = query.Include(a => a.Tags)
                .Include(a => a.Version)
                .Include(a => a.Organization)
                .ThenInclude(o => o.Users);

            if (filters.OrganizationId != Guid.Empty)
            {
                query = query.Where(a => a.Organization.Id == filters.OrganizationId);
            }

            if (filters.VersionId != Guid.Empty)
            {
                query = query.Where(a => a.Version.Id == filters.VersionId);
            }

            query = query.Where(a => a.Organization.Users.Any(u => u.Id == userId));

            return (query.Skip(offset).Take(limit).ToList(), query.Count());
        }

        public AdContainerModel UpdateAdContainer(AdContainerModel updatedAdContainer)
        {
            _context.AdContainer.Update(updatedAdContainer);
            _context.SaveChanges();
            return updatedAdContainer;
        }

        public int DeleteAdContainer(AdContainerModel adContainer)
        {
            _context.AdContainer.Remove(adContainer);
            return _context.SaveChanges();
        }

        public int DeleteAdContainersForVersion(Guid versionId)
        {
            _context.AdContainer.RemoveRange(_context.AdContainer.Include(a => a.Version)
                .Where(a => a.Version.Id == versionId));
            return _context.SaveChanges();
        }
    }
}