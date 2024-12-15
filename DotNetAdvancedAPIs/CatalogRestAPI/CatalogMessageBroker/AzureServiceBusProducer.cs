using Azure.Messaging.ServiceBus;
using Domain.Entities;
using Domain.ExternalServiceInterfaces;
using System.Text.Json;

namespace CatalogMessageBroker
{
    public class AzureServiceBusProducer : IProductMessageBroker
    {

        static string connStr = @"Endpoint=sb://cartbus.servicebus.windows.net/;SharedAccessKeyName=p1;SharedAccessKey=88gaYA6PEdsU5TBaNJ+IQKJDgg9ibzX8O+ASbNrvgCA=;EntityPath=cart-queue";
        static string QueueName = "cart-queue";
               

        public async Task<bool> SendProductChangeMessageAsync(Product Product)
        {
            var messageBody = JsonSerializer.Serialize(Product);
            return await SendMessageAsync(messageBody);
        }

        public async Task<bool> SendTestMessageAsync() {

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
