using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WeeklyNotification.App.DAL.Entities;
using WeeklyNotification.App.Models;
using WeeklyNotification.App.Services;

namespace WeeklyNotification.App
{
    public static class WeeklyNotificationFunction
    {
        public static IServiceProvider ServiceProvider = Bootstrap.ConfigureServices();

        [FunctionName("WeeklyNotificationFunction")]
        public static async Task Run([TimerTrigger("0 */1 * * * *"
#if DEBUG
                , RunOnStartup = true
#endif
            )]
            TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"Weekly notification sending start: {DateTime.UtcNow}");
                var interests = new List<CustomerInvestementInfo>();
                interests.AddRange(await ServiceProvider.GetService<IInvestmentOrderService<FiatInvestmentOrder>>()
                    .GetInterests());
                interests.AddRange(await ServiceProvider.GetService<IInvestmentOrderService<FiatFundInvestmentOrder>>()
                    .GetInterests());
                interests.AddRange(await ServiceProvider
                    .GetService<IInvestmentOrderService<CryptoFundInvestmentOrder>>().GetInterests());
                interests.AddRange(await ServiceProvider.GetService<IInvestmentOrderService<FiatFundInvestmentOrder>>()
                    .GetInterests());
                var notificationService = ServiceProvider.GetService<INotificationService>();
                await notificationService.SendNotifications(interests);
                
                log.LogInformation($"Weekly notification sending done: {DateTime.UtcNow}");
            }
            catch (Exception ex)
            {
                log.LogError($"Function error: {ex.Message} . Trace: {JsonConvert.SerializeObject(ex)}");
            }
        }
    }
}