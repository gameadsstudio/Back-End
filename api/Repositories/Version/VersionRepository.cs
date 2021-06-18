using System;
using System.Collections.Generic;
using System.Linq;
using api.Contexts;
using api.Models.Version;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories.Version
{
    public class VersionRepository : IVersionRepository
    {
        private readonly ApiContext _context;

        public VersionRepository(ApiContext context)
        {
            _context = context;
        }

        public VersionModel AddNewVersion(VersionModel version)
        {
            _context.Version.Add(version);
            _context.SaveChanges();
            return version;
        }

        public VersionModel GetVersionById(Guid id)
        {
            return _context.Version
                .Include(v => v.Game)
                .ThenInclude(g => g.Organization)
                .SingleOrDefault(a => a.Id == id);
        }

        public (IList<VersionModel> versions, int count) GetVersions(int offset, int limit, VersionFiltersDto filters)
        {
            IQueryable<VersionModel> query = _context.Version.OrderBy(a => a.SemVer);

            query = query.Where(a => a.Game.Id == filters.GameId);

            if (!string.IsNullOrEmpty(filters.Name))
            {
                query = query.Where(a => a.Name == filters.Name);
            }
            if (!string.IsNullOrEmpty(filters.SemVer))
            {
                query = query.Where(a => a.SemVer == filters.SemVer);
            }

            return (query
                .Skip(offset)
                .Take(limit)
                .ToList(), query.Count());
        }

        public VersionModel UpdateVersion(VersionModel updatedVersion)
        {
            _context.Update(updatedVersion);
            _context.SaveChanges();
            return updatedVersion;
        }

        public int DeleteVersion(VersionModel version)
        {
            _context.Version.Remove(version);
            return _context.SaveChanges();
        }
    }
}