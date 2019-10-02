using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using WeeklyNotification.App.Models;

namespace WeeklyNotification.App.ServiceProviders
{
    public interface IZeuxProvider
    {
        Task SendInAppNotifications(IEnumerable<NotificationMessage> notifications);
    }

    public class ZeuxProvider : IZeuxProvider
    {
        public async Task SendInAppNotifications(IEnumerable<NotificationMessage> notifications)
        {
//            Log.ForContext("class", "ZeuxProvider")
//                .ForContext("method", "SendPushNotification")
//                .ForContext("params", new {customerId, title, content, data}, true)
//                .Information("Send Push Notification for specific customer.");

            var apiKey = Environment.GetEnvironmentVariable("Zeux_ApiKey");
            var apiRequestDelay = int.Parse(Environment.GetEnvironmentVariable("ZeuxApi_RequestDelay"));
            var client = new HttpClient();
            
            client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("Zeux_ApiBaseAddress"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var requestDatas =
                    notifications.Select(n => new
                        {CustomerIds = new List<int>(){n.CustomerId}, Title = n.Message.Title, Message = n.Message.Body})
                ;
            foreach (var data in requestDatas)
            {
                var response = await client.PostAsJsonAsync("service/sendbroadcastinappnotifications", data);
                response.EnsureSuccessStatusCode();
                Thread.Sleep(apiRequestDelay);
            }
            
//            Log.ForContext("class", "ZeuxProvider")
//                .ForContext("method", "SendPushNotification")
//                .ForContext("response", responseContent, true)
//                .Information("Send Push Notification for specific customer. Response.");

        }
    }
}