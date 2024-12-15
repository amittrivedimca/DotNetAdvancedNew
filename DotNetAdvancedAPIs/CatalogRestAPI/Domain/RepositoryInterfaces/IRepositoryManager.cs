using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductDomain.RepositoryInterfaces
{
    public interface IRepositoryManager
    {
        public ICategoryRepository CategoryRepository  { get; }
        public IProductRepository ProductRepository { get; }
    }
}
