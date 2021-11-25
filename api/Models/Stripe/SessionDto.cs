using System.ComponentModel.DataAnnotations;

namespace api.Models.Stripe
{
    public class SessionDto
    {
        [Required]
        public long UnitAmount { get; set; }

        [Required]
        public string Currency { get; set; }

        public string Customer { get; set; }
    }
}
