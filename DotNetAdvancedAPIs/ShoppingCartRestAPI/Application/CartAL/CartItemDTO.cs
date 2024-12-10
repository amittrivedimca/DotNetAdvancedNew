using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CartAL
{
    public class CartItemDTO
    {
        public string CartId { get; set; }
        public int ItemId { get; set; }
        [Required(ErrorMessage = "Name is required")]        
        public string Name { get; set; }
        public ItemImageInfoDTO ItemImage { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<CartItem, CartItemDTO>();
                CreateMap<CartItemDTO, CartItem>();
            }
        }
    }
}
