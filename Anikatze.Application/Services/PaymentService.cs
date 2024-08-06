using Anikatze.Application.Infrastracture;
using Anikatze.Application.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using Anikatze.Application.Dtos;
using Microsoft.Azure.Management.ServiceBus.Fluent.Queue.Definition;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using static Stripe.Checkout.SessionService;
using System.Threading.Tasks;

namespace Anikatze.Application.Services
{
    public class PaymentService
    {
        private readonly AnikatzeContext _db;
        private readonly IMapper _mapper;
        private readonly string _baseUrl;

        public PaymentService(AnikatzeContext db, IMapper mapper, string baseUrl)
        {
            _db = db;
            _mapper = mapper;
            _baseUrl = baseUrl;
        }

        public IQueryable<Payment> Payments => _db.Set<Payment>().AsQueryable();


        public IWithSession CreateSession(List<CheckoutItemDto> CheckoutItemDtolist)
        {
            string successURL = _baseUrl + "payment/success";
            string failedURL = _baseUrl + "payment/failed";

            StripeConfiguration.ApiKey = "sk_test_51OjNEYAzssJMNlZ1WZuRMbperRdc1N4qhYIl9TrgS" +
                "7Sjku1eDxHBrqOJcm1wzFmtrnNcpPuEr9E4qTeS1tRMbi7T009s2s1lVW";

            var sessionItemList = new List<LineItemOptions>();

            foreach (var CheckoutItemDto in CheckoutItemDtolist)
            {
                sessionItemList.Add(CreateLineItem(CheckoutItemDto));
            }
            var options = new SessionCreateParams
            {
                PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                Mode = "payment",
                SuccessUrl = successURL,
                CancelUrl = failedURL,
                LineItemOptions = sessionItemList,
            };

            var service = new SessionService();
            //return service.Create(options);
            return null;
        }

        private LineItemOptions CreateLineItem(CheckoutItemDto checkoutItemDto)
        {

            var lineItemOptions = new LineItemOptions {
                Name = checkoutItemDto.Name,
                PriceData = new PriceDataOptions {
                    Currency = "eur",
                    UnitAmount = (long)(checkoutItemDto.Price * 100),
                    ProductData = new CourseDataOption {
                        Name = checkoutItemDto.Name
                    }
                }
            };
            return lineItemOptions;
        }
    }
}
