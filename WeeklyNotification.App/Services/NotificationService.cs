using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WeeklyNotification.App.DAL.Contracts;
using WeeklyNotification.App.DAL.Entities;
using WeeklyNotification.App.Models;
using WeeklyNotification.App.ServiceProviders;

namespace WeeklyNotification.App.Services
{
    public interface INotificationService
    {
        Task SendNotifications(IEnumerable<CustomerInvestmentInfo> infos);
    }

    public class NotificationService : INotificationService
    {
        private readonly IRepository<CustomerMessage> _customerMessageRepository;
        private readonly INotificationHubProvider _notificationHubProvider;
        private readonly ILogger _logger;
        private readonly IZeuxProvider _zeuxProvider;

        public NotificationService(IRepository<CustomerMessage> customerMessageRepository,
            IZeuxProvider zeuxProvider, INotificationHubProvider notificationHubProvider,
            ILogger<NotificationService> logger)
        {
            _customerMessageRepository = customerMessageRepository;
            _zeuxProvider = zeuxProvider;
            _notificationHubProvider = notificationHubProvider;
            _logger = logger;
        }

        public async Task SendNotifications(IEnumerable<CustomerInvestmentInfo> infos)
        {
            var ids = new List<int>() {2};
            infos = infos.Where(i => ids.Contains(i.Customer.Id));

            _logger.LogInformation($"Sending {infos.Count()} weekly notifications");
            var notificationMessages = infos.Select(i => new NotificationMessage()
            {
                CustomerId = i.Customer.Id,
                DeviceToken = i.Customer.DeviceToken,
                DeviceType = i.Customer.DeviceType,
                Message = GetMessageContent(i)
            });

            var batchSize = 500;
            var batches = GetChunked(notificationMessages.ToList(), batchSize);
            int failedAttempts = 0;
            foreach (var batch in batches)
            {
                try
                {
//                    await SaveMessages(batch);
//                    await _notificationHubProvider.SendPushNotifications(batch);
//                    await _zeuxProvider.SendInAppNotifications(batch);
                }
                catch (Exception e)
                {
                    _logger.LogError($"Push sending failed. Ex:{e.Message}");
                    failedAttempts++;
                }
            }

            if (failedAttempts > 0)
            {
                _logger.LogWarning($"{failedAttempts} out of {batches.Count()} sending attempts failed.");
            }
        }

        private async Task SaveMessages(IEnumerable<NotificationMessage> notifications)
        {
            await _customerMessageRepository.BulkInsert(notifications.Select(n => new CustomerMessage()
            {
                Customer = n.CustomerId,
                Title = n.Message.Title,
                Message = n.Message.Body,
                CreateTime = DateTime.UtcNow
            }));
        }

        private static List<List<NotificationMessage>> GetChunked(List<NotificationMessage> source, int batchSize)
        {
            return source
                .Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index / batchSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        private static MessageContent GetMessageContent(CustomerInvestmentInfo info)
        {
            return new MessageContent()
            {
                Title =
                    $"You've earned £{Math.Round(info.InterestEarned, 3).ToString("N3", CultureInfo.InvariantCulture)} interest so far",
                Body = info.AmountNPV + info.InterestEarned >= 1000
                    ? "Invite your friends to earn together"
                    : "Deposit more to earn more"
            };
        }
    }
}