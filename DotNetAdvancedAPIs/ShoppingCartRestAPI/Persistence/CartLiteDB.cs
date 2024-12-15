using Domain.BusinessLogic;
using Domain.Entities;
using LiteDB;

namespace Persistence
{
    /// <summary>
    /// https://www.litedb.org/docs/getting-started/
    /// </summary>
    internal class CartLiteDB
    {
        private LiteDatabase GetDB()
        {
            // Open database (or create if doesn't exist)
            return new LiteDatabase(@"C:\Temp\CartAPIData.db");
        }

        private ILiteCollection<Cart> GetCartsCollection(LiteDatabase db)
        {
            // Get a collection (or create, if doesn't exist)
            return db.GetCollection<Cart>("carts");
        }

        public bool InsertCart(Cart cart)
        {
            using (var db = GetDB())
            {
                var cartsCollection = GetCartsCollection(db);
                // Insert new document (Id will be auto-incremented)
                cartsCollection.Insert(cart);
            }

            return true;
        }

        public bool UpdateCart(Cart cart)
        {
            using (var db = GetDB())
            {
                var cartsCollection = GetCartsCollection(db);
                // Insert new document (Id will be auto-incremented)
                cartsCollection.Update(cart);
            }

            return true;
        }

        public Cart GetCart(string cartId)
        {
            using (var db = GetDB())
            {
                var cartsCollection = GetCartsCollection(db);
                return cartsCollection.Query()
                    .Where(x => x.CartId == cartId)
                    .Include(c => c.CartItems)
                    .FirstOrDefault();
            }
        }

        public IEnumerable<Cart> GetAllCarts()
        {
            using (var db = GetDB())
            {
                var cartsCollection = GetCartsCollection(db);
                return cartsCollection.Query().ToList();
            }
        }

    }
}
