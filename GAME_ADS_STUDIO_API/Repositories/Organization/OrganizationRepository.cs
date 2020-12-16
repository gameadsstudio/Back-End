using System;
using System.Collections.Generic;
using System.Linq;
using GAME_ADS_STUDIO_API.Contexts;
using GAME_ADS_STUDIO_API.Models.Organization;

namespace GAME_ADS_STUDIO_API.Repositories.Organization
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly GasContext _context;

        public OrganizationRepository(GasContext context)
        {
            _context = context;
        }

        public int AddNewOrganization(OrganizationModel organization)
        {
            _context.Organization.Add(organization);
            return _context.SaveChanges();
        }

        public int DeleteOrganization(OrganizationModel organization)
        {
            _context.Organization.Remove(organization);
            return _context.SaveChanges();
        }

        public OrganizationPublicModel[] GetOrganizations(int offset, int limit) {
            return _context.Organization.OrderBy(p => p.org_id).Select(p => new OrganizationPublicModel{
                    media_id = p.media_id,
                    org_name = p.org_name,
                    org_email = p.org_email,
                    org_city = p.org_city,
                    org_url = p.org_url,
                    org_type = p.org_type
            })
                .Skip(offset)
                .Take(limit)
                .ToArray();
        }

        public OrganizationModel GetOrganizationById(int id)
        {
            return _context.Organization.SingleOrDefault(a => a.org_id == id);
        }

        public int UpdateOrganization(OrganizationUpdateModel updatedOrganization, OrganizationModel target)
        {
            _context.Entry(target).CurrentValues.SetValues(updatedOrganization);
            return _context.SaveChanges();
        }
    }
}
