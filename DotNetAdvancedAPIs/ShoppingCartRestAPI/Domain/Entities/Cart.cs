using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductDomain.Entities
{
    public class Cart
    {
        public string CartId { get; set; }
        public List<CartItem> CartItems { get; set; }
        public Cart()
        {
            CartItems = new List<CartItem>();
        }
    }
}
