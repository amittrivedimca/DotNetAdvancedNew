using Azure.Messaging.ServiceBus;
using Domain.Entities;
using Domain.ExternalServiceInterfaces;

namespace CartMessageBroker
{
    public class AzureServiceBusReceiver : ICartMessageBroker
    {       

        static string connStr = @"Endpoint=sb://cartbus.servicebus.windows.net/;SharedAccessKeyName=p1;SharedAccessKey=88gaYA6PEdsU5TBaNJ+IQKJDgg9ibzX8O+ASbNrvgCA=;EntityPath=cart-queue";
        static string QueueName = "cart-queue";

        public async Task<List<CartItem>> ReceiveProductMessageAsync()
        {
            List<string> messages = await ReceiveMessageAsync();
            List<CartItem> cartItems = new List<CartItem>();
            foreach (var message in messages)
            {

            }
            return cartItems;
        }

        public async Task<List<string>> ReceiveTestMessageAsync()
        {
            return await ReceiveMessageAsync();
        }

        private async Task<List<string>> ReceiveMessageAsync()
        {
            ServiceBusClient client = new ServiceBusClient(connStr);
            ServiceBusReceiver receiver = client.CreateReceiver(QueueName);
            List<string> messages = new List<string>();
            try
            {
                var receivedMessages = await receiver.ReceiveMessagesAsync(50, new TimeSpan(0, 1, 0));
                var count = receivedMessages.Count();
                foreach (var message in receivedMessages)
                {
                    string id = message.MessageId;
                    string messageData = message.Body.ToString();
                    messages.Add(messageData);
                    await receiver.CompleteMessageAsync(message);
                }
            }
            catch (Exception ex)
            {
                return messages;
            }
            finally
            {
                await receiver.CloseAsync();
                await receiver.DisposeAsync();
                await client.DisposeAsync();
            }
            return messages;
        }
    }
}
