using Application.CartAL;
using AutoMapper;
using Domain.ExternalServiceInterfaces;
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

        public ApplicationManager(IMapper mapper, IRepositoryManager repositoryManager,ICartMessageBroker messageBroker)
        {
            _lazyCartProvider = new Lazy<ICartProvider>(() => new CartProvider(mapper, repositoryManager, messageBroker));
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
