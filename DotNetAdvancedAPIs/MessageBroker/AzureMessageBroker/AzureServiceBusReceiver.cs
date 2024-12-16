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

            if (!await IsAnyActiveMessage())
            {
                return messages;
            }

            ServiceBusClientOptions clientOptions = new ServiceBusClientOptions();
            clientOptions.TransportType = ServiceBusTransportType.AmqpWebSockets;
            clientOptions.RetryOptions = new ServiceBusRetryOptions()
            {
                Delay = TimeSpan.FromSeconds(0.8),
                MaxDelay = TimeSpan.FromSeconds(10),
                MaxRetries = 3,
            };

            ServiceBusClient client = new ServiceBusClient(connStr, clientOptions);

            // create a processor that we can use to process the messages
            //ServiceBusProcessor processor = client.CreateProcessor(QueueName, new ServiceBusProcessorOptions());

            ServiceBusReceiver receiver = client.CreateReceiver(QueueName);            
            
            try
            {

                //// add handler to process messages
                //processor.ProcessMessageAsync += Processor_ProcessMessageAsync;
                //// add handler to process any errors
                //processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
                //// start processing 
                //await processor.StartProcessingAsync();

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
                //await processor.DisposeAsync();
                await client.DisposeAsync();
            }
            return messages;
        }

        //private Task Processor_ProcessErrorAsync(ProcessErrorEventArgs args)
        //{
        //    return Task.CompletedTask;
        //}

        //private async Task Processor_ProcessMessageAsync(ProcessMessageEventArgs args)
        //{
        //    string body = args.Message.Body.ToString();
        //    // complete the message. message is deleted from the queue. 
        //    await args.CompleteMessageAsync(args.Message); 
        //}

        private async Task<bool> IsAnyActiveMessage()
        {
            ServiceBusAdministrationClient administrationClient = new ServiceBusAdministrationClient(connStr);
            var queueInfo = await administrationClient.GetQueueRuntimePropertiesAsync(QueueName);
            var activeMessages = queueInfo.Value.ActiveMessageCount;
            return activeMessages > 0;
        }
    }
}
