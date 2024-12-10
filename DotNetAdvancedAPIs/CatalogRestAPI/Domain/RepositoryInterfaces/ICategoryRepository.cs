using Domain.Entities;
using HelperUtils;

namespace Domain.RepositoryInterfaces
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
