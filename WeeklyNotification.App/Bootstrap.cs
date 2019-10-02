using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
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
				.AddScoped(typeof(INotificationService), typeof(NotificationService))
				.AddScoped(typeof(IInvestmentOrderService<>), typeof(InvestmentOrderService<>));
			services.AddLogging();
			return services.BuildServiceProvider();
		}
	}
}
