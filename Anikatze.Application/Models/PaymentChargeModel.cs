using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anikatze.Application.Models
{
    public class PaymentChargeModel
    {
        public ChargeCreateOptions PrepareChargeModel(
            long amount,
            string description,
            string currency,
            string customerId,
            string email)
        {
            var model = new ChargeCreateOptions
            {
                Amount = amount * 100,
                Description = description,
                Currency = currency,
                Customer = customerId,
                ReceiptEmail = email
            };
            return model;
        }
    }
}
