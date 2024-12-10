using Domain.Entities;
using Domain.RepositoryInterfaces;
using HelperUtils;

namespace Application.CategoryAL
{
    public class CategoryProvider : ICategoryProvider
    {
        private readonly IMapper _mapper;
        private ICategoryRepository _categoryRepository;

        public CategoryProvider(IMapper mapper, IRepositoryManager repositoryManager)
        {
            _mapper = mapper;
            _categoryRepository = repositoryManager.CategoryRepository;
        }

        public Task<DBOperationStatus> AddAsync(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<CategoryDTO,Category>(categoryDTO);
            return _categoryRepository.AddAsync(category);
        }

        public Task<DBOperationStatus> DeleteAsync(int id)
        {
            return _categoryRepository.DeleteAsync(id);
        }

        public IEnumerable<CategoryDTO> GetAll()
        {
            var list = _categoryRepository.GetAll();            
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(list);
        }

        public async Task<(DBOperationStatus, CategoryDTO)> GetById(int id)
        {
            var category = await _categoryRepository.GetByID(id);
            if (category != null)
            {
                var dto = _mapper.Map<Category, CategoryDTO>(category);
                return (DBOperationStatus.Success, dto);
            }
            return (DBOperationStatus.NotFound, null);
        }

        public Task<DBOperationStatus> UpdateAsync(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<CategoryDTO, Category>(categoryDTO);
            return _categoryRepository.UpdateAsync(category);
        }
    }
}
