using HelperUtils;

namespace Application.ProductAL
{
    public interface IProductProvider 
    {
        Task<DBOperationStatus> AddAsync(ProductDTO productDTO);
        Task<DBOperationStatus> DeleteAsync(int id);
        Task<PagedList<ProductShortInfoDTO>> GetAllAsync(ProductsFilter filter);
        Task<(DBOperationStatus, ProductDTO)> GetById(int id);
        Task<DBOperationStatus> UpdateAsync(ProductDTO productDTO);
    }
}
