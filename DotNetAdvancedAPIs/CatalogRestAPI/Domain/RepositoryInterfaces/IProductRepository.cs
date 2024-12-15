using ProductDomain.Entities;
using HelperUtils;

namespace ProductDomain.RepositoryInterfaces
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
