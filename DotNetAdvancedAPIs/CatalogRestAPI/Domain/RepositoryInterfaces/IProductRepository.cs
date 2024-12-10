using Domain.Entities;
using HelperUtils;

namespace Domain.RepositoryInterfaces
{
    public interface IProductRepository
    {
        Task<PagedList<Product>> GetAllAsync(ProductsFilter filter);
        Task<Product?> GetByID(int id);
        Task<DBOperationStatus> AddAsync(Product product);
        Task<DBOperationStatus> UpdateAsync(Product product);
        Task<DBOperationStatus> DeleteAsync(int id);
    }
}
