using Domain.RepositoryInterfaces;
using Persistence.Repositories;

namespace Persistence
{
    internal class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<ICartRepository> lazyCartRepository = new Lazy<ICartRepository>();

        public RepositoryManager()
        {
            lazyCartRepository = new Lazy<ICartRepository>(() => new CartRepository());
        }

        public ICartRepository CartRepository => lazyCartRepository.Value;
    }
}
