using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeeklyNotification.App.DAL.Contracts;
using WeeklyNotification.App.DAL.Repositories;
using WeeklyNotification.App.ServiceProviders;
using WeeklyNotification.App.Services;

[assembly: FunctionsStartup(typeof(WeeklyNotification.App.Startup))]
namespace WeeklyNotification.App
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            builder.Services.AddDbContext<DAL.ZeuxDbContext>(options => options.UseSqlServer(connectionString))
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped(typeof(IZeuxProvider), typeof(ZeuxProvider))
                .AddScoped(typeof(INotificationHubProvider), typeof(NotificationHubProvider))
                .AddScoped(typeof(IFundInvestmentOrderService<>), typeof(FundInvestmentOrderService<>))
                .AddScoped(typeof(IDepositInvestmentOrderService<>), typeof(DepositInvestmentOrderService<>))
                .AddScoped(typeof(INotificationService), typeof(NotificationService))
                .AddScoped(typeof(ICustomerInvestmentCalculationService),
                    typeof(CustomerInvestmentCalculationCalculationService)).AddLogging();
        }
    }
}