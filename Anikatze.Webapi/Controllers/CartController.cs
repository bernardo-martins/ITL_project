using Anikatze.Application.Commands;
using Anikatze.Application.Dtos;
using Anikatze.Application.Models;
using Anikatze.Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AspShowcase.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _service;
        private readonly IMapper _mapper;  // braucht builder.Services.AddAutoMapper(typeof(MappingProfile));

        public CartController(IMapper mapper, CartService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public ActionResult <CartItemDto> GetCartItems()
        {
            var cartItems = _service.CartItems;
            return Ok(cartItems);
        }

        [HttpPost]
        public IActionResult AddtoCart([FromBody] AddtoCartDto addtoCartDto)
        {
            try
            {
                _service.AddtoCart(addtoCartDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        


        [HttpDelete]
        public IActionResult DeleteCartItem(int cartItemId)
        {
            try
            {
                _service.DeleteCartItem(cartItemId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}

