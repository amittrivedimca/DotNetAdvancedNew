﻿using ProductDomain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductDomain.ExternalServiceInterfaces
{
    public interface ICartMessageBroker
    {
        Task<List<string>> ReceiveTestMessageAsync();
        Task<List<CartItem>> ReceiveProductMessageAsync();
    }
}
