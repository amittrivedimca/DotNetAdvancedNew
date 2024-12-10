using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CartItem
    {
        public int ItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public ItemImageInfo ItemImage { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
    }
}
