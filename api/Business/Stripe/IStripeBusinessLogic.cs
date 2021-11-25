using Stripe;
using Stripe.Checkout;
using api.Models.Stripe;

namespace api.Business.Stripe
{
    public interface IStripeBusinessLogic
    {
        Customer CreateAccount(string name, string email, string description);
        Session CreateSession(SessionDto session);
    }
}
