using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Enums;

namespace api.Models.Advertisement
{
    [Table("advertisement")]
    public class AdvertisementModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        // [ForeignKey("AdvertisementTypeId")]
        // public AdvertisementTypeModel AdvertisementType { get; set; }
        
        // [ForeignKey("CampaignId")]
        // public CampaignModel Campaign { get; set; }
        
        public int AgeMin { get; set; }
        
        public int AgeMax { get; set; }
        
        public AdvertisementStatus Status { get; set; }
        
        public DateTimeOffset DateCreation { get; set; }
        
        public DateTimeOffset DateUpdate { get; set; }
    }
}