using Domain.Entities;
using Domain.RepositoryInterfaces;
using HelperUtils;
using Microsoft.EntityFrameworkCore;
using Persistence.DB;

namespace Persistence
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogDB _catalogDB;
        public CategoryRepository(CatalogDB catalogDB) {
            _catalogDB = catalogDB;
        }

        public IEnumerable<Category> GetAll()
        {
            return _catalogDB.Categories.AsNoTracking();
        }

        public Task<Category?> GetByID(int id)
        {
            return _catalogDB.Categories.FirstOrDefaultAsync(c => c.ID == id);
        }

        public async Task<DBOperationStatus> AddAsync(Category category)
        {
            await _catalogDB.Categories.AddAsync(category);
            int cnt = await _catalogDB.SaveChangesAsync();
            return DBOperationStatus.Success;
        }

        public async Task<DBOperationStatus> UpdateAsync(Category category)
        {
            Category? categoryToUpdate = await GetByID(category.ID);
            if (categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;
                categoryToUpdate.Image = category.Image;
                await _catalogDB.SaveChangesAsync();
                return DBOperationStatus.Success;
            }
            return DBOperationStatus.NotFound;
        }

        public async Task<DBOperationStatus> DeleteAsync(int id)
        {
            Category? categoryToDelete = await GetByID(id);
            if(categoryToDelete != null)
            {
                RemoveProductsFromCategory(categoryToDelete);
                _catalogDB.Categories.Remove(categoryToDelete);
                await _catalogDB.SaveChangesAsync();
                return DBOperationStatus.Success;
            }
            return DBOperationStatus.NotFound;
        }

        private void RemoveProductsFromCategory(Category category)
        {
            if (category.Products != null && category.Products.Any())
            {
                foreach (var prod in category.Products)
                {
                    category.Products.Remove(prod);
                }
            }
        }
    }
}
