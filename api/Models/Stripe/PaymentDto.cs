using System.ComponentModel.DataAnnotations;

namespace api.Models.Stripe
{
    public class PaymentDto
    {
        [Required]
        public string Id { get; set; }
    }
}
