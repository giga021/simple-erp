using Autofac;
using Autofac.Extensions.DependencyInjection;
using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using Knjizenje.API.Infrastructure.AutofacModules;
using Knjizenje.Persistence.Context;
using Knjizenje.Persistence.EventSourcing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Knjizenje.Service
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var logger = NLog.LogManager.LoadConfiguration("NLog.config").GetCurrentClassLogger();
			try
			{
				var hostBuilder = new HostBuilder()
				 .ConfigureAppConfiguration((h, c) => ConfigureAppConfiguration(h, c, args))
				 .UseServiceProviderFactory(new AutofacServiceProviderFactory())
				 .ConfigureContainer<ContainerBuilder>(ConfigureServices)
				 .ConfigureLogging(ConfigureLogging);

				var host = hostBuilder.UseConsoleLifetime().Build();
				await InitializeDatabaseAsync(host.Services);
				await InitializeEventStoreAsync(host.Services);
				await host.RunAsync();
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

		private static void ConfigureAppConfiguration(HostBuilderContext hostContext, IConfigurationBuilder config, string[] args)
		{
			config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true);
			config.AddEnvironmentVariables();

			if (args != null)
			{
				config.AddCommandLine(args);
			}
		}

		private static void ConfigureServices(HostBuilderContext hostContext, ContainerBuilder builder)
		{
			//options
			var appSettings = new AppSettings();
			hostContext.Configuration.Bind(appSettings);
			var options = Options.Create(appSettings);
			builder.RegisterInstance(options);

			//logging
			var loggerFactory = new LoggerFactory();
			loggerFactory.AddNLog();
			builder.RegisterInstance(loggerFactory).As<ILoggerFactory>();
			GlobalDiagnosticsContext.Set("connectionString", appSettings.ConnectionString);

			//modules
			builder.RegisterModule(new ApplicationModule());
			builder.RegisterModule(new EventStoreModule());
			builder.RegisterModule(new MediatrModule());
			builder.RegisterModule(new ServiceBusModule());
		}

		private static void ConfigureLogging(HostBuilderContext hostContext, ILoggingBuilder logging) { }

		private static async Task InitializeDatabaseAsync(IServiceProvider services)
		{
			using (var scope = services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<KnjizenjeContext>();
				KnjizenjeContextSeed seeder = new KnjizenjeContextSeed();
				await seeder.SeedAsync(context);
			}
		}

		private static async Task InitializeEventStoreAsync(IServiceProvider services)
		{
			using (var scope = services.CreateScope())
			{
				var projectionManager = scope.ServiceProvider.GetRequiredService<ProjectionsManager>();
				var env = services.GetRequiredService<IConfiguration>();
				var options = services.GetRequiredService<IOptions<AppSettings>>();
				var credentials = new UserCredentials(
					options.Value.EVENTSTORE_USERNAME,
					options.Value.EVENTSTORE_PASSWORD);
				await EventStoreInitializer.InitializeAsync(projectionManager, credentials);
			}
		}
	}
}
