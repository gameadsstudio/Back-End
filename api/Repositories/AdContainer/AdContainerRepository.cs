using System;
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

        public (AdContainerModel[], int) GetAdContainersByOrganizationId(int offset, int limit, Guid orgId)
        {
            return (_context.AdContainer.OrderBy(p => p.Id)
                    .Include(a => a.Tags)
                    .Where(p => p.Organization.Id == orgId)
                    .Skip(offset)
                    .Take(limit)
                    .ToArray(),
                _context.AdContainer.Count(p => p.Organization.Id == orgId));
        }

        public AdContainerModel UpdateAdContainer(AdContainerModel updatedAdContainer)
        {
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