using Anikatze.Application;
using Anikatze.Application.Dtos;
using Anikatze.Application.Models;
using Anikatze.Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Anikatze.Application;
using Microsoft.Azure.Management.AppService.Fluent.Models;
using Stripe;
using System.Net;

namespace Anikatze.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly PaymentService _service;

        public PaymentController(IMapper mapper, PaymentService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost("/checkout-session")]
        public IActionResult Checkoutlist([FromBody] List<CheckoutItemDto> CheckoutItemDtolist)
        {
            var session = _service.CreateSession(CheckoutItemDtolist);

            var stripeResponse = new Stripe.StripeResponse(HttpStatusCode.OK, null, "code");
            return Ok(stripeResponse);
        }
    }
}
