using ProductDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductDomain.ExternalServiceInterfaces
{
    public interface IProductMessageBroker
    {
        Task<bool> SendTestMessageAsync();
        Task<bool> SendProductChangeMessageAsync(Product Product);
    }        
        
}
