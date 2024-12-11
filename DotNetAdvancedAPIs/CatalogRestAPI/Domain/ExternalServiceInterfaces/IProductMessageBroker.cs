using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ExternalServiceInterfaces
{
    public interface IProductMessageBroker
    {
        Task<bool> SendMessageAsync();
    }
}
