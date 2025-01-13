using Application.CategoryAL;
using Application.ProductAL;
using ProductDomain.ExternalServiceInterfaces;
using ProductDomain.RepositoryInterfaces;

namespace Application
{
    public class ApplicationManager : IApplicationManager
    {    
        private readonly Lazy<ICategoryProvider> lazyCategoryProvider = new Lazy<ICategoryProvider>();
        private readonly Lazy<IProductProvider> lazyProductProvider = new Lazy<IProductProvider>();

        public ApplicationManager(IMapper mapper, IRepositoryManager repositoryManager, IProductMessageBroker messageBroker)
        {
            lazyCategoryProvider = new Lazy<ICategoryProvider>(() => new CategoryProvider(mapper, repositoryManager));
            lazyProductProvider = new Lazy<IProductProvider>(() => new ProductProvider(mapper, repositoryManager, messageBroker));
        }

        public ICategoryProvider CategoryProvider => lazyCategoryProvider.Value;

        public IProductProvider ProductProvider => lazyProductProvider.Value;
    }
}
