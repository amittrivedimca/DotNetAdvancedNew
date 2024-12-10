namespace Domain.Entities
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; }
        public Category ParentCategory { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
