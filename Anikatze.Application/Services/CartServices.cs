using Anikatze.Application.Infrastracture;
using Anikatze.Application.Models;
using AutoMapper;
using Anikatze.Application.Dtos;

namespace Anikatze.Application.Services
{
    public class CartService
    {
        private readonly AnikatzeContext _db;
        private readonly IMapper _mapper;
        public CartService(AnikatzeContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public IQueryable<Cart> Carts => _db.Set<Cart>().AsQueryable();
        public IQueryable<Course> Courses => _db.Set<Course>().AsQueryable();
        public IQueryable<CartItem> CartItems => _db.Set<CartItem>().AsQueryable();


        public CartItem GetCartItems(int CartItemId)
        {
            var cartItems = _mapper.ProjectTo<CartItemDto>(_db.CartItems
                .Where(ci => ci.CartItemID == CartItemId)).FirstOrDefault();
            if (cartItems == null)
            {
                return null;
            }
            var cartItem = _mapper.Map<CartItem>(cartItems);
            return cartItem;
        }

        public void AddtoCart(AddtoCartDto addtoCartDto)
        {
            Cart cart = Carts.FirstOrDefault(c => c.CartID == addtoCartDto.CourseId);

        }

        public void DeleteCartItem(int CartItemId)
        {
            CartItem cartItem = CartItems.FirstOrDefault(ci => ci.CartItemID == CartItemId);
            if (cartItem != null)
            {
                _db.Remove(cartItem);
                _db.SaveChanges();
            }
        }

    }
}