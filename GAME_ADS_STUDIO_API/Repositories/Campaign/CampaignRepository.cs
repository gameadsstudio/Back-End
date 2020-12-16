using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAME_ADS_STUDIO_API.Contexts;
using GAME_ADS_STUDIO_API.Models.Campaign;

namespace GAME_ADS_STUDIO_API.Repositories.Campaign
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly GasContext _context;

        public CampaignRepository(GasContext context)
        {
            _context = context;
        }

        public int AddNewCampaign(CampaignModel Campaign)
        {
            _context.Campaign.Add(Campaign);
            return _context.SaveChanges();
        }

        public int DeleteCampaign(CampaignModel Campaign)
        {
            _context.Campaign.Remove(Campaign);
            return _context.SaveChanges();
        }

        public CampaignPublicModel[] GetCampaigns(int offset, int limit)
        {
            return _context.Campaign.OrderBy(p => p.cpg_id).Select(p => new CampaignPublicModel
            {
                cpg_id = p.cpg_id,
                org_id = p.org_id,
                cpg_name = p.cpg_name,
                cpg_age_min = p.cpg_age_min,
                cpg_age_max = p.cpg_age_max,
                cpg_type = p.cpg_type,
                cpg_status = p.cpg_status,
                cpg_date_begin = p.cpg_date_begin,
                cpg_date_end = p.cpg_date_end,
            })
                .Skip(offset)
                .Take(limit)
                .ToArray();
        }

        public CampaignModel GetCampaignById(int id)
        {
            return _context.Campaign.SingleOrDefault(a => a.cpg_id == id);
        }

        public int UpdateCampaign(CampaignUpdateModel updatedCampaign, CampaignModel target)
        {
            _context.Entry(target).CurrentValues.SetValues(updatedCampaign);
            return _context.SaveChanges();
        }
    }
}
