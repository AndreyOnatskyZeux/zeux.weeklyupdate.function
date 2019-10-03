using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using WeeklyNotification.App.DAL.Contracts;
using WeeklyNotification.App.DAL.Repositories;
using WeeklyNotification.App.ServiceProviders;
using WeeklyNotification.App.Services;


namespace WeeklyNotification.App
{
	public static class Bootstrap
	{
		public static IServiceProvider ConfigureServices()
		{
			var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

			var services = new ServiceCollection()
				.AddDbContext<DAL.ZeuxDbContext>(options => options.UseSqlServer(connectionString))
				.AddScoped(typeof(IRepository<>), typeof(Repository<>))
				.AddScoped(typeof(IZeuxProvider), typeof(ZeuxProvider))
				.AddScoped(typeof(INotificationHubProvider), typeof(NotificationHubProvider))
				.AddScoped(typeof(IFundInvestmentOrderService<>), typeof(FundInvestmentOrderService<>))
				.AddScoped(typeof(IDepositInvestmentOrderService<>), typeof(DepositInvestmentOrderService<>))
				.AddScoped(typeof(INotificationService), typeof(NotificationService))
				.AddScoped(typeof(ICustomerInvestmentCalculationService), typeof(CustomerInvestmentCalculationCalculationService))
				.AddScoped(typeof(ILogger<>), typeof(Logger<>))
				.AddLogging();
			return services.BuildServiceProvider();
		}
	}
}
