using ProductDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CartAL
{
    public interface ICartProvider
    {
        Task<CartDTO> GetCart(string cartId);        
        Task<IEnumerable<CartItemDTO>> GetCartItems(string cartId);
        public CartDTO AddOrUpdateItem(string cartId, CartItemDTO item);
        public bool RemoveItem(string cartId, int itemId);
        Task<IEnumerable<string>> ReceiveAndProcessProductChangeMessages();
    }
}
