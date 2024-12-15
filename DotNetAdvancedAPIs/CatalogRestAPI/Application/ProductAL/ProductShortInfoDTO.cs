using ProductDomain.Entities;

namespace Application.ProductAL
{
    public class ProductShortInfoDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Product, ProductShortInfoDTO>();
            }
        }
    }
}
