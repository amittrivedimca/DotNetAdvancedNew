using Application.CategoryAL;
using Application.ProductAL;
using Domain.ExternalServiceInterfaces;
using Domain.RepositoryInterfaces;

namespace Application
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly IMapper _mapper;
        private ICategoryRepository _categoryRepository;
        private readonly Lazy<ICategoryProvider> lazyCategoryProvider = new Lazy<ICategoryProvider>();
        private readonly Lazy<IProductProvider> lazyProductProvider = new Lazy<IProductProvider>();

        public ApplicationManager(IMapper mapper, IRepositoryManager repositoryManager, IProductMessageBroker messageBroker)
        {
            _mapper = mapper;
            _categoryRepository = repositoryManager.CategoryRepository;
            lazyCategoryProvider = new Lazy<ICategoryProvider>(() => new CategoryProvider(mapper, repositoryManager));
            lazyProductProvider = new Lazy<IProductProvider>(() => new ProductProvider(mapper, repositoryManager, messageBroker));
        }

        public ICategoryProvider CategoryProvider => lazyCategoryProvider.Value;

        public IProductProvider ProductProvider => lazyProductProvider.Value;
    }
}
