using ProductDomain.BusinessLogic;
using ProductDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductDomain.UnitTests.BusinessLogic
{
    public class CartBLTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldAssignNewCartProperty()
        {
            var cartBL = new CartBL();
            Assert.IsTrue(cartBL.IsCreate);
            Assert.IsNotNull(cartBL.Cart);
            Assert.IsNotEmpty(cartBL.Cart.CartId);
        }

        [Test]
        public void ShouldNotAssignNewCartProperty()
        {
            Cart cart = new Cart() { CartId = "id" };
            var cartBL = new CartBL(cart);
            Assert.IsFalse(cartBL.IsCreate);
            Assert.IsNotNull(cartBL.Cart);
        }

        [Test]
        public void ShouldAddNewItem()
        {            
            var cartBL = new CartBL();
            cartBL.AddItem(new CartItem() { ItemId = 1, Name = "item1" });
            Assert.IsTrue(cartBL.Cart.CartItems.Count > 0);
        }

    }
}
