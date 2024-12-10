using Domain.RepositoryInterfaces;
using Persistence.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<ICategoryRepository> lazyCategoryRepository = new Lazy<ICategoryRepository>();
        private readonly Lazy<IProductRepository> lazyProductRepository = new Lazy<IProductRepository>();
        
        public RepositoryManager(CatalogDB catalogDB)
        {
            lazyCategoryRepository = new Lazy<ICategoryRepository>(() => new CategoryRepository(catalogDB));
            lazyProductRepository = new Lazy<IProductRepository>(() => new ProductRepository(catalogDB));
        }

        public ICategoryRepository CategoryRepository => lazyCategoryRepository.Value;
        public IProductRepository ProductRepository => lazyProductRepository.Value;
    }
}
