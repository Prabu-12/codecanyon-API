using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using QuizplusApi.Models;
using QuizplusApi.Models.Others;
using QuizplusApi.Models.Quiz;
using Stripe;
using Stripe.Checkout;

namespace QuizplusApi.Controllers
{
    public class PaymentsController:Controller
    {
        private readonly IConfiguration _config;
        private readonly IStripeClient client;
        private readonly ISqlRepository<SiteSettings> _siteSettingsRepo;
        private readonly ISqlRepository<QuizTopic> _quizTopicRepo;
        SiteSettings objSettings;
        public PaymentsController(IConfiguration config,
                                AppDbContext context,
                                ISqlRepository<SiteSettings> siteSettingsRepo,
                                ISqlRepository<QuizTopic> quizTopicRepo)
        {
            _quizTopicRepo=quizTopicRepo;
            _siteSettingsRepo=siteSettingsRepo;
            objSettings=_siteSettingsRepo.SelectSingle();
            StripeConfiguration.ApiKey=objSettings.StripeSecretKey;
            this.client=new StripeClient(objSettings.StripeSecretKey);
            _config=config;
        }

        [HttpGet("checkout-session")]
        public async Task<Session> GetCheckoutSession(string sessionId)
        {
            var service=new SessionService(this.client);
            var session = await service.GetAsync(sessionId);
            return session;
        }

        [HttpPost("create-checkout-session-registration")]
        public ActionResult CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = objSettings.RegistrationPrice*100,
                            Currency = objSettings.Currency.ToLower(),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "One Time Registration",
                            },

                        },
                        Quantity = 1,                                          
                    },
                },
                Mode = "payment",
                SuccessUrl = objSettings.ClientUrl+"?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = objSettings.ClientUrl,               
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [HttpPost("pay-for-quiz")]
        public ActionResult PayforQuiz(int quizTopicId)
        {
            int price=_quizTopicRepo.SelectById(quizTopicId).QuizPrice;
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = price*100,
                            Currency = objSettings.Currency.ToLower(),
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Assessment Price",
                            },

                        },
                        Quantity = 1,                                          
                    },
                },
                Mode = "payment",
                SuccessUrl = objSettings.ClientUrl+"/dashboard?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = objSettings.ClientUrl+"/dashboard",               
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        
    }
}