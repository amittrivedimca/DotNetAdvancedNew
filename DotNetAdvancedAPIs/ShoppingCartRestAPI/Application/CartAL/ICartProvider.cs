using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CartAL
{
    public interface ICartProvider
    {
        public CartDTO GetCart(string cartId);        
        public IEnumerable<CartItemDTO> GetCartItems(string cartId);
        public CartDTO AddItem(string cartId, CartItemDTO item);
        public bool RemoveItem(string cartId, int itemId);
    }
}
