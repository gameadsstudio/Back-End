using System;
using System.Collections.Generic;
using AutoMapper;
using Stripe;
using Stripe.Checkout;
using api.Models.Stripe;

namespace api.Business.Stripe
{
    public class StripeBusinessLogic : IStripeBusinessLogic
    {
        private readonly IMapper _mapper;

        public StripeBusinessLogic(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Customer CreateAccount(
            string name,
            string email,
            string description
        )
        {
            CustomerService service = new CustomerService();
            CustomerCreateOptions options = new CustomerCreateOptions
            {
                Description = description,
                Email = email,
                Name = name
            };

            return service.Create(options);
        }

        public Charge CreateCharge(ChargesDto charges, string description)
        {
            ChargeService service = new ChargeService();
            ChargeCreateOptions options = new ChargeCreateOptions
            {
                Amount = charges.Amount,
                Currency = charges.Currency,
                Customer = charges.Customer,
                Description = description,
                ReceiptEmail = charges.ReceiptEmail,
                Source = charges.Source
            };

            return service.Create(options);
        }

        public Session CreateSession(SessionDto session)
        {
            ProductCreateOptions productCreateOptions = new ProductCreateOptions
            {
                Name = "Custom payment",
                Description = "A custom payment amount"
            };
            PriceCreateOptions priceCreateOptions = new PriceCreateOptions
            {
                Product = new ProductService().Create(productCreateOptions).Id,
                UnitAmount = session.UnitAmount,
                Currency = session.Currency
            };
            SessionService service = new SessionService();
            SessionCreateOptions options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = new PriceService().Create(
                            priceCreateOptions
                        ).Id,
                        Quantity = 1
                    }
                },
                Customer = session.Customer,
                Mode = "payment",
                SuccessUrl = Environment.GetEnvironmentVariable(
                    "STRIPE_SUCCESS_URL"
                ).TrimEnd('/') + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = Environment.GetEnvironmentVariable(
                    "STRIPE_CANCEL_URL"
                ).TrimEnd('/')
            };

            return service.Create(options);
        }
    }
}
