using AutoMapper;
using Domain.BusinessLogic;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CartAL
{
    public class CartProvider : ICartProvider
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public CartProvider(IMapper mapper, IRepositoryManager repositoryManager)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
        }

        public CartDTO AddItem(string cartId, CartItemDTO item)
        {
            var cart = _repositoryManager.CartRepository.GetCart(cartId);
            CartBL cartBL = null;
            
            if (cart != null)
            {
                cartBL = new CartBL(cart);
            }
            else
            {
                cartBL = new CartBL();
            }

            try
            {
                CartItem cartItem = _mapper.Map<CartItemDTO, CartItem>(item);

                cartBL.AddItem(cartItem);

                if (cartBL.IsCreate)
                {
                    _repositoryManager.CartRepository.InsertCart(cartBL.Cart);
                }
                else
                {
                    _repositoryManager.CartRepository.UpdateCart(cartBL.Cart);
                }

                return _mapper.Map<Cart,CartDTO>(cartBL.Cart); 
            }
            catch (Exception ex) {
                throw;
            }
        }

        public CartDTO GetCart(string cartId)
        {
            var cart = _repositoryManager.CartRepository.GetCart(cartId);
            if (cart != null)
            {
                return _mapper.Map<Cart, CartDTO>(cart);
            }
            return null;
        }

        public IEnumerable<CartItemDTO> GetCartItems(string cartId)
        {
            var cart = _repositoryManager.CartRepository.GetCart(cartId);
            if (cart != null && cart.CartItems != null)
            {
                return _mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemDTO>>(cart.CartItems);
            }
            return new List<CartItemDTO>();
        }

        public bool RemoveItem(string cartId, int itemId)
        {
            var cart = _repositoryManager.CartRepository.GetCart(cartId);
            CartBL cartBL = null;
            if (cart != null)
            {
                cartBL = new CartBL(cart);
                cartBL.RemoveItem(itemId);
                _repositoryManager.CartRepository.UpdateCart(cart);
                return true;
            }
            return false;
        }
    }
}
