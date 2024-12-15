using ProductDomain.ExternalServiceInterfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartMessageBroker
{
    public static class DependencyInjection
    {
        //public static IServiceCollection AddMessageBrokerServices(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddTransient<ICartMessageBroker, AzureServiceBusReceiver>();
        //    return services;
        //}
    }
}
