using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeeklyNotification.App.DAL.Entities;
using WeeklyNotification.App.Models;
using WeeklyNotification.App.Services;

namespace WeeklyNotification.App
{
    public class WeeklyNotificationFunction
    {
        private readonly INotificationService _notificationService;
        private readonly IFundInvestmentOrderService<CryptoFundInvestmentOrder> _cryptoFundOrdersService;
        private readonly IFundInvestmentOrderService<FiatFundInvestmentOrder> _fiatFundOrderService;
        private readonly IDepositInvestmentOrderService<CryptoDepositInvestmentOrder> _cryptoDepositOrderService;
        private readonly IDepositInvestmentOrderService<FiatDepositInvestmentOrder> _fiatDepositOrderService;
        private readonly ICustomerInvestmentCalculationService _calculationService;

        public WeeklyNotificationFunction(
            INotificationService notificationService, 
            IFundInvestmentOrderService<CryptoFundInvestmentOrder> cryptoFundOrdersService,
            IFundInvestmentOrderService<FiatFundInvestmentOrder> fiatFundOrderService,
            IDepositInvestmentOrderService<CryptoDepositInvestmentOrder> cryptoDepositOrderService,
            IDepositInvestmentOrderService<FiatDepositInvestmentOrder> fiatDepositOrderService,
            ICustomerInvestmentCalculationService calculationService
            )
        {
            _notificationService = notificationService;
            _cryptoFundOrdersService = cryptoFundOrdersService;
            _fiatFundOrderService = fiatFundOrderService;
            _cryptoDepositOrderService = cryptoDepositOrderService;
            _fiatDepositOrderService = fiatDepositOrderService;
            _calculationService = calculationService;
        }
        
        [FunctionName("WeeklyNotificationFunction")]
        public async Task Run([TimerTrigger("0 0 12 * * THU"
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
                orderModels.AddRange(await _cryptoFundOrdersService.GetInvestmentOrderModels());
                orderModels.AddRange(await _fiatFundOrderService.GetInvestmentOrderModels());
                orderModels.AddRange(await _cryptoDepositOrderService.GetInvestmentOrderModels());
                orderModels.AddRange(await _fiatDepositOrderService.GetInvestmentOrderModels());
                
                await _notificationService.SendNotifications(await _calculationService.GetInvestmentInfos(orderModels));
                
                log.LogInformation($"Weekly notification sending done: {DateTime.UtcNow}");
            }
            catch (Exception ex)
            {
                log.LogError($"Function error: {ex.Message} . Trace: {JsonConvert.SerializeObject(ex)}");
            }
        }
    }
}