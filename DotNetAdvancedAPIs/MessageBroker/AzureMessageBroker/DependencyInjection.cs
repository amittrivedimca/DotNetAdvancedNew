using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductDomain.ExternalServiceInterfaces;

namespace AzureMessageBroker
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCatalogMessageBrokerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IProductMessageBroker, AzureServiceBusProducer>();
            return services;
        }

        public static IServiceCollection AddCartMessageBrokerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICartMessageBroker, AzureServiceBusReceiver>();            
            return services;
        }

    }
}
