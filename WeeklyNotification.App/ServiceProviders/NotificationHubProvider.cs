using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using WeeklyNotification.App.Models;

namespace WeeklyNotification.App.ServiceProviders
{
    public interface INotificationHubProvider
    {
        Task SendPushNotifications(IEnumerable<NotificationMessage> messages);
    }

    public class NotificationHubProvider : INotificationHubProvider
    {
        private readonly string _serviceBusConnectionString =
            Environment.GetEnvironmentVariable("AzureNotificationHub_ServiceBusConnectionString");

        private readonly string _queueName = Environment.GetEnvironmentVariable("AzureNotificationHub_QueueName");


        public async Task SendPushNotifications(IEnumerable<NotificationMessage> messages)
        {
            var serviceBusMessages = messages.ToArray().Select(d => GetServiceBusMessage(d)).ToList();
            var serviceBusQueueClient = new QueueClient(_serviceBusConnectionString, _queueName);
            await serviceBusQueueClient.SendAsync(serviceBusMessages).ConfigureAwait(false);
            await serviceBusQueueClient.CloseAsync();
        }

        private Message GetServiceBusMessage(NotificationMessage message)
        {
            return new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
        }
    }
}