using Domain.Entities;
using Domain.RepositoryInterfaces;
using HelperUtils;
using Microsoft.EntityFrameworkCore;
using Persistence.DB;

namespace Persistence
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogDB _catalogDB;
        public ProductRepository(CatalogDB catalogDB)
        {
            _catalogDB = catalogDB;
        }

        public async Task<DBOperationStatus> AddAsync(Product product)
        {
            await _catalogDB.Products.AddAsync(product);
            int cnt = await _catalogDB.SaveChangesAsync();
            return DBOperationStatus.Success;
        }

        public async Task<DBOperationStatus> DeleteAsync(int id)
        {
            Product? productToDelete = await GetByID(id);
            if (productToDelete != null)
            {
                _catalogDB.Products.Remove(productToDelete);
                await _catalogDB.SaveChangesAsync();
                return DBOperationStatus.Success;
            }
            return DBOperationStatus.NotFound;
        }

        public async Task<PagedList<Product>> GetAllAsync(ProductsFilter filter)
        {
            PagedList<Product> pagedList;            
            var products = _catalogDB.Products.AsNoTracking();
            int totalRecords = await products.CountAsync();

            if (filter != null)
            {
                if (filter.CategoryID > 0)
                {
                    products = products.Where(p => p.CategoryId == filter.CategoryID);
                }

                if (filter.IsValidPageFilter())
                {
                    products = products.Skip(filter.Skip).Take(filter.PageSize);
                }
                pagedList = new PagedList<Product>(products, totalRecords, filter.PageNumber, filter.PageSize);
            }
            else
            {
                pagedList = new PagedList<Product>(products, totalRecords, 0, 0);
            }
            return pagedList;
        }

        public Task<Product?> GetByID(int id)
        {
            return _catalogDB.Products.FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<DBOperationStatus> UpdateAsync(Product product)
        {
            Product? productToUpdate = await GetByID(product.ID);
            if (productToUpdate != null)
            {
                productToUpdate.Name = product.Name;
                productToUpdate.Image = product.Image;
                productToUpdate.CategoryId = product.CategoryId;
                await _catalogDB.SaveChangesAsync();
                return DBOperationStatus.Success;
            }
            return DBOperationStatus.NotFound;
        }
    }
}
