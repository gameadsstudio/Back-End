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
            return _context.AdContainer
                .Include(a => a.Tags)
                .Include(a => a.Organization)
                .SingleOrDefault(a => a.Id == id);
        }

        public AdContainerModel GetAdContainerByName(string name)
        {
            return _context.AdContainer
                .Include(a => a.Tags)
                .SingleOrDefault(a => a.Name == name);
        }

        public int CountAdContainers()
        {
            return _context.AdContainer.Count();
        }

        public (List<AdContainerModel>, int) GetAdContainersByOrganizationId(int offset, int limit,
            Guid orgId, Guid userId)
        {
            var query = _context.AdContainer.OrderByDescending(p => p.DateCreation)
                .Include(a => a.Tags)
                .Include(a => a.Organization)
                .ThenInclude(o => o.Users)
                .Where(p => p.Organization.Id == orgId && p.Organization.Users.Any(u => u.Id == userId));

            return (query.Skip(offset)
                        .Take(limit)
                        .ToList(),
                    query.Count());
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
    }
}