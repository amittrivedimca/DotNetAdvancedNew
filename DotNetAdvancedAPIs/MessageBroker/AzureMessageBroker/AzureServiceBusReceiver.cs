using Azure.Messaging.ServiceBus;
using ProductDomain.Entities;
using ProductDomain.ExternalServiceInterfaces;
using System.Text.Json;

namespace AzureMessageBroker
{
    public class AzureServiceBusReceiver : BaseAzureMessageBroker,ICartMessageBroker
    {
        public async Task<List<CartItem>> ReceiveProductMessageAsync()
        {
            List<string> messages = await ReceiveMessageAsync();
            List<CartItem> cartItems = new List<CartItem>();
            foreach (var message in messages)
            {
                Product product = JsonSerializer.Deserialize<Product>(message);
                cartItems.Add(new CartItem()
                {
                    ItemId = product.ID,
                    Name = product.Name,
                    Price = (double)product.Price,
                    Quantity = product.Amount
                });
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
