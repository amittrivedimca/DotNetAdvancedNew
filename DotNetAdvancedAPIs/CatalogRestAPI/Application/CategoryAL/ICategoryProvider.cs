using HelperUtils;

namespace Application.CategoryAL
{
    public interface ICategoryProvider
    {
        IEnumerable<CategoryDTO> GetAll();
        Task<(DBOperationStatus, CategoryDTO)> GetById(int id);
        Task<DBOperationStatus> AddAsync(CategoryDTO category);
        Task<DBOperationStatus> UpdateAsync(CategoryDTO category);
        Task<DBOperationStatus> DeleteAsync(int id);
    }
}
