using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
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
           
            List<string> messages = new List<string>();

            if (!await IsAnyActiveMessages())
            {
                return messages;
            }

            ServiceBusClientOptions clientOptions = new ServiceBusClientOptions();
            clientOptions.RetryOptions = new ServiceBusRetryOptions()
            {
                Delay = TimeSpan.FromSeconds(0.8),
                MaxDelay = TimeSpan.FromSeconds(10),
                MaxRetries = 3,
            };

            ServiceBusClient client = new ServiceBusClient(connStr, clientOptions);            
            ServiceBusReceiver receiver = client.CreateReceiver(QueueName);            
            
            try
            {                
                var receivedMessages = await receiver.ReceiveMessagesAsync(5, new TimeSpan(0, 0, 10));                
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

        private async Task<bool> IsAnyActiveMessages()
        {
            ServiceBusAdministrationClient administrationClient = new ServiceBusAdministrationClient(connStr);
            var queueInfo = await administrationClient.GetQueueRuntimePropertiesAsync(QueueName);
            var activeMessages = queueInfo.Value.ActiveMessageCount;
            return activeMessages > 0;
        }
    }
}
