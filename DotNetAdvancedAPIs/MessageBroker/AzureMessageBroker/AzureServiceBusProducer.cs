using Azure.Messaging.ServiceBus;
using ProductDomain.Entities;
using ProductDomain.ExternalServiceInterfaces;
using System.Text.Json;

namespace AzureMessageBroker
{
    public class AzureServiceBusProducer : BaseAzureMessageBroker, IProductMessageBroker
    {
        public async Task<bool> SendProductChangeMessageAsync(Product Product)
        {
            var messageBody = JsonSerializer.Serialize(Product);
            return await SendMessageAsync(messageBody);
        }

        public async Task<bool> SendTestMessageAsync()
        {

            await SendMessageAsync("1 >> test");
            await SendMessageAsync("2 >> test");
            return true;
        }

        private async Task<bool> SendMessageAsync(string messageBody)
        {
            ServiceBusClient client = new ServiceBusClient(connStr);
            ServiceBusSender sender = client.CreateSender(QueueName);
            try
            {
                await sender.SendMessageAsync(new ServiceBusMessage(messageBody));
            }
            catch (Exception ex)
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
