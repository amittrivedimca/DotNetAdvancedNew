using AutoMapper;
using ProductDomain.BusinessLogic;
using ProductDomain.Entities;
using ProductDomain.ExternalServiceInterfaces;
using ProductDomain.RepositoryInterfaces;

namespace Application.CartAL
{
    public class CartProvider : ICartProvider
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly ICartMessageBroker _messageBroker;

        public CartProvider(IMapper mapper, IRepositoryManager repositoryManager, ICartMessageBroker messageBroker)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _messageBroker = messageBroker;
        }

        public CartDTO AddOrUpdateItem(string cartId, CartItemDTO item)
        {
            var cart = _repositoryManager.CartRepository.GetCart(cartId);
            CartItem cartItem = _mapper.Map<CartItemDTO, CartItem>(item);
            Cart updatedCart = AddOrUpdateItem(cartId, cartItem);
            return _mapper.Map<Cart, CartDTO>(updatedCart);

        }

        private Cart AddOrUpdateItem(string cartId, CartItem cartItem)
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

            cartBL.AddOrUpdateItem(cartItem);

            try
            {
                if (cartBL.IsCreate)
                {
                    _repositoryManager.CartRepository.InsertCart(cartBL.Cart);
                }
                else
                {
                    _repositoryManager.CartRepository.UpdateCart(cartBL.Cart);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return cartBL.Cart;
        }

        public async Task<CartDTO> GetCart(string cartId)
        {
            await ReceiveAndProcessProductChangeMessages();
            var cart = _repositoryManager.CartRepository.GetCart(cartId);
            if (cart != null)
            {
                return _mapper.Map<Cart, CartDTO>(cart);
            }
            return null;
        }

        public async Task<IEnumerable<CartItemDTO>> GetCartItems(string cartId)
        {
            await ReceiveAndProcessProductChangeMessages();
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

        public async Task<IEnumerable<string>> ReceiveAndProcessProductChangeMessages() {
            var products = await _messageBroker.ReceiveProductMessageAsync();
            List<string> cartIds = new List<string>();
            if (products.Count > 0)
            {                
                var carts = _repositoryManager.CartRepository.GetAllCarts();

                foreach (var product in products)
                {
                    foreach (var cart in carts)
                    {
                        var cartItemToUpdate = cart.CartItems.FirstOrDefault(c => c.ItemId == product.ItemId);
                        if(cartItemToUpdate != null)
                        {                            
                            cartItemToUpdate.Name = product.Name;
                            cartItemToUpdate.Price = product.Price;
                            cartItemToUpdate.Quantity = product.Quantity;
                            AddOrUpdateItem(cart.CartId, cartItemToUpdate);
                            if (!cartIds.Contains(cart.CartId))
                            {
                                cartIds.Add(cart.CartId);
                            }
                        }
                    }
                }
            }

            return cartIds;
        }
    }
}
