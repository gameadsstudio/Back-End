using System.ComponentModel.DataAnnotations;

namespace api.Models.Stripe
{
    public class ChargesDto
    {
        [Required]
        public long Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Source { get; set; }

        public string ReceiptEmail { get; set; }

        public string Customer { get; set; }
    }
}
