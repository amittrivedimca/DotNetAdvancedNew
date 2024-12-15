using ProductDomain.Entities;

namespace ProductDomain.RepositoryInterfaces
{
    public interface ICartRepository
    {
        public Cart GetCart(string cartId);
        public bool InsertCart(Cart cart);
        public bool UpdateCart(Cart cart);
        public IEnumerable<Cart> GetAllCarts();
        //public bool AddItem(string cartId, CartItem item);
        //public bool RemoveItem(string cartId, int itemId);
    }
}
