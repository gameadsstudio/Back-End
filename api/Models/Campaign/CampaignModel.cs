using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Campaign
{
    [Table("campaign")]
    public class CampaignModel
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [Column("org_id")]
        [ForeignKey("OrganizationModel")]
        public Guid OrganizationId { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Column("age_min")]
        public string AgeMin { get; set; }
        [Column("age_max")]
        public string AgeMax { get; set; }
        [Column("type")]
        public string Type { get; set; }
        [Column("status")]
        public string Status { get; set; }
        [Column("date_begin")]
        public DateTimeOffset DateBegin { get; set; }
        [Column("date_end")]
        public DateTimeOffset DateEnd { get; set; }
        [Column("date_creation")]
        public DateTimeOffset DateCreation { get; set; }
        [Column("date_update")]
        public DateTimeOffset DateUpdate { get; set; }
    }
}
