using Application.CartAL;
using AutoMapper;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly Lazy<ICartProvider> _lazyCartProvider;

        public ApplicationManager(IMapper mapper, IRepositoryManager repositoryManager)
        {
            _lazyCartProvider = new Lazy<ICartProvider>(() => new CartProvider(mapper, repositoryManager));
        }

        public ICartProvider CartProvider
        {
            get
            {
                return _lazyCartProvider.Value;
            }
        }
    }
}
