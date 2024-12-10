using Domain.Entities;

namespace Domain.RepositoryInterfaces
{
    public interface ICartRepository
    {
        public Cart GetCart(string cartId);
        public bool InsertCart(Cart cart);
        public bool UpdateCart(Cart cart);
        //public bool AddItem(string cartId, CartItem item);
        //public bool RemoveItem(string cartId, int itemId);
    }
}
