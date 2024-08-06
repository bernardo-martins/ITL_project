using Stripe.Checkout;
using System.Collections.Generic;

namespace Anikatze.Application.Services
{
    internal class SessionCreateParams
    {
        public List<string> PaymentMethodTypes { get; set; }
        public string Mode { get; set; }
        public string SuccessUrl { get; set; }
        public string CancelUrl { get; set; }
        public List<LineItemOptions> LineItemOptions { get; set; }
    }

    internal class LineItemOptions
    {
        public string Name { get; set; }
        public PriceDataOptions PriceData { get; set; }
    }

    internal class PriceDataOptions
    {
        public string Currency { get; set; }
        public long UnitAmount { get; set; }
        public CourseDataOption ProductData { get; set; }
    }

    internal class CourseDataOption
    {
        public string Name { get; set; }
    }
}
