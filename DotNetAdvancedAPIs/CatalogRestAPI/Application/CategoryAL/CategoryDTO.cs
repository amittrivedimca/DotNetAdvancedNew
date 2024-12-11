using Domain.Entities;

namespace Application.CategoryAL
{
    public class CategoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public CategoryDTO? ParentCategory { get; private set; }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<Category, CategoryDTO>();
                CreateMap<CategoryDTO,Category>();
            }
        }
    }
}
