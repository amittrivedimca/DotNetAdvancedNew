using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureMessageBroker
{
    public class BaseAzureMessageBroker
    {
       protected readonly static string connStr = @"Endpoint=sb://cartbus.servicebus.windows.net/;SharedAccessKeyName=p1;SharedAccessKey=88gaYA6PEdsU5TBaNJ+IQKJDgg9ibzX8O+ASbNrvgCA=;EntityPath=cart-queue";
       protected readonly static string QueueName = "cart-queue";
    }
}
