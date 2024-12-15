using ProductDomain.Entities;
using HelperUtils;

namespace ProductDomain.RepositoryInterfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Task<Category?> GetByID(int id);
        Task<DBOperationStatus> AddAsync(Category category);
        Task<DBOperationStatus> UpdateAsync(Category category);
        Task<DBOperationStatus> DeleteAsync(int id);
    }
}
