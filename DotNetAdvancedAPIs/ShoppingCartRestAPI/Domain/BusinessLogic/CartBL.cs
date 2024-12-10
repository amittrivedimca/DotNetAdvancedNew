using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BusinessLogic
{
    public class CartBL
    {
        public Cart Cart { get; private set; }
        public bool IsCreate;
        public CartBL()
        {
            IsCreate = true;
            CreateCart();
        }

        public CartBL(Cart cart)
        {
            IsCreate = false;
            Cart = cart;
        }

        public void CreateCart()
        {
            Cart = new Cart();            
            Cart.CartId = GenerateCartId();            
        }

        public void AddItem(CartItem cartItem)
        {
            Cart.CartItems.Add(cartItem);
        }

        public bool RemoveItem(int itemId)
        {
            CartItem? cartItemToRemove = Cart.CartItems.FirstOrDefault(c => c.ItemId == itemId);
            if (cartItemToRemove != null)
            {
                Cart.CartItems.Remove(cartItemToRemove);
                return true;
            }
            return false;
        }
        
        private string GenerateCartId()
        {
            return Guid.NewGuid().ToString("N");
        }

    }
}
