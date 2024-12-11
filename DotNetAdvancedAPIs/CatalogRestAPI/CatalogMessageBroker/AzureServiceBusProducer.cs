using Azure.Messaging.ServiceBus;
using Domain.ExternalServiceInterfaces;

namespace CatalogMessageBroker
{
    public class AzureServiceBusProducer : IProductMessageBroker
    {
        
        static string connStr = @"Endpoint=sb://cartbus.servicebus.windows.net/;SharedAccessKeyName=p1;SharedAccessKey=88gaYA6PEdsU5TBaNJ+IQKJDgg9ibzX8O+ASbNrvgCA=;EntityPath=cart-queue";
        static string QueueName = "cart-queue";
        

        public async Task<bool> SendMessageAsync() {
            ServiceBusClient client = new ServiceBusClient(connStr);
            ServiceBusSender sender = client.CreateSender(QueueName);
            try
            {                
                await sender.SendMessageAsync(new ServiceBusMessage("1 >> test"));
                await sender.SendMessageAsync(new ServiceBusMessage("2 >> test"));
            }
            catch(Exception ex)
            {
                return false;
            }
            finally
            {                
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
                
            return true;
        }
    }
}
