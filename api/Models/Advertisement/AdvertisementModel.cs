using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Campaign;
using api.Models.Media;

namespace api.Models.Advertisement
{
    [Table("advertisement")]
    public class AdvertisementModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public CampaignModel Campaign { get; set; }
        
        public MediaModel Media { get; set; }

        public int AgeMin { get; set; }
        
        public int AgeMax { get; set; }
        
        public DateTimeOffset DateCreation { get; set; }
        
        public DateTimeOffset DateUpdate { get; set; }
    }
}