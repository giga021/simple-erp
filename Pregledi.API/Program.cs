using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Web;
using Pregledi.Persistence.Context;
using Pregledi.Persistence.EventSourcing;
using System;
using System.Threading.Tasks;

namespace Pregledi.API
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
			try
			{
				var host = CreateWebHostBuilder(args).Build();
				await InitializeDatabaseAsync(host);
				await InitializeEventStoreAsync(host);
				host.Run();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				NLog.LogManager.Shutdown();
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args).ConfigureLogging(logging =>
			{
				logging.ClearProviders();
			}).UseNLog().UseStartup<Startup>();
		}

		private static async Task InitializeDatabaseAsync(IWebHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				using (var context = services.GetRequiredService<PreglediContext>())
				{
					var seeder = new PreglediContextSeed();
					await seeder.SeedAsync(context);
				}
			}
		}

		private static async Task InitializeEventStoreAsync(IWebHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var projectionManager = services.GetRequiredService<ProjectionsManager>();
				var options = services.GetRequiredService<IOptions<AppSettings>>();
				var credentials = new UserCredentials(options.Value.EVENTSTORE_USERNAME, options.Value.EVENTSTORE_PASSWORD);
				await EventStoreInitializer.InitializeAsync(projectionManager, credentials);
			}
		}
	}
}
