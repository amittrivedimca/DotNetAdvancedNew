using ProductDomain.ExternalServiceInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogMessageBroker
{
    public static class DependencyInjection
    {
        //public static IServiceCollection AddMessageBrokerServices(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddTransient<IProductMessageBroker, AzureServiceBusProducer>();
        //    return services;
        //}
    }
}
