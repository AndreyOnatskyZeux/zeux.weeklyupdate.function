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
        public static async Task Run([TimerTrigger("0 0 12 * * THU"
#if DEBUG
                , RunOnStartup = true
#endif
            )]
            TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"Weekly notification sending start: {DateTime.UtcNow}");
                var orderModels = new List<InvestmentOrderModel>();
                orderModels.AddRange(await ServiceProvider.GetService<IFundInvestmentOrderService<CryptoFundInvestmentOrder>>()
                    .GetInvestmentOrderModels());
                orderModels.AddRange(await ServiceProvider.GetService<IFundInvestmentOrderService<FiatFundInvestmentOrder>>()
                    .GetInvestmentOrderModels());
                orderModels.AddRange(await ServiceProvider.GetService<IDepositInvestmentOrderService<CryptoDepositInvestmentOrder>>()
                    .GetInvestmentOrderModels());
                orderModels.AddRange(await ServiceProvider.GetService<IDepositInvestmentOrderService<FiatDepositInvestmentOrder>>()
                    .GetInvestmentOrderModels());
                var calculationService = ServiceProvider.GetService<ICustomerInvestmentCalculationService>();
                var notificationService = ServiceProvider.GetService<INotificationService>();
                await notificationService.SendNotifications(await calculationService.GetInvestmentInfos(orderModels));
                
                log.LogInformation($"Weekly notification sending done: {DateTime.UtcNow}");
            }
            catch (Exception ex)
            {
                log.LogError($"Function error: {ex.Message} . Trace: {JsonConvert.SerializeObject(ex)}");
            }
        }
    }
}