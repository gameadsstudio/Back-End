using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Advertisement
{
    [Table("advertisement")]
    public class AdvertisementModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int AgeMin { get; set; }
        public int AgeMax { get; set; }
        public String Status { get; set; }
        public DateTimeOffset DateCreation { get; set; }
        public DateTimeOffset DateUpdate { get; set; }
    }
}