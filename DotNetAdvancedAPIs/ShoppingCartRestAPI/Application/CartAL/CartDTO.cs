using AutoMapper;
using Domain.Entities;

namespace Application.CartAL
{
    public class CartDTO
    {
        public string CartId { get; set; }

        public List<CartItem> CartItems { get; set; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Cart, CartDTO>();
                CreateMap<CartDTO, Cart>();
            }
        }
    }
}
