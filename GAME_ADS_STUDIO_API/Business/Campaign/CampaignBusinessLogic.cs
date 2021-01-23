using GAME_ADS_STUDIO_API.Repositories.Campaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using GAME_ADS_STUDIO_API.Configuration;
using GAME_ADS_STUDIO_API.Contexts;
using Microsoft.Extensions.Options;
using GAME_ADS_STUDIO_API.Models.Campaign;

namespace GAME_ADS_STUDIO_API.Business.Campaign
{
    public class CampaignBusinessLogic : ICampaignBusinessLogic
    {
        private readonly ICampaignRepository _repository;
        private readonly AppSettings _appSettings;

        public CampaignBusinessLogic(IOptions<AppSettings> appSettings)
        {
            //_repository = new CampaignRepository(context);
            _appSettings = appSettings.Value;
        }

        public CampaignModel AddNewCampaign(CampaignCreationModel newCampaign)
        {
            var Campaign = new CampaignModel();

            Campaign.Org_id = newCampaign.Org_id;
            Campaign.Cpg_name = newCampaign.Cpg_name;
            Campaign.Cpg_age_min = newCampaign.Cpg_age_min;
            Campaign.Cpg_age_max = newCampaign.Cpg_age_max;
            Campaign.Cpg_type = newCampaign.Cpg_type;
            Campaign.Cpg_status = newCampaign.Cpg_status;
            Campaign.Cpg_date_begin = newCampaign.Cpg_date_begin;
            Campaign.Cpg_date_end = newCampaign.Cpg_date_end;
            Campaign.Cpg_date_creation = DateTime.Now;
            Campaign.Cpg_date_update = DateTime.Now;

            return Campaign;
        }

        public int DeleteCampaignById(int id)
        {
            return 1;
        }

        public int UpdateCampaignById(int id, CampaignUpdateModel updatedCampaign)
        {
            return 1;
        }
    }
}
