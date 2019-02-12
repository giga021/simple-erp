using Autofac;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Projections;
using EventStore.ClientAPI.SystemData;
using Microsoft.Extensions.Options;
using Pregledi.Domain.Repositories;
using System;
using System.Net;

namespace Pregledi.API.Infrastructure.AutofacModules
{
	public class EventStoreModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<EventStoreLogger>().As<ILogger>().InstancePerLifetimeScope();
			builder.RegisterType<Persistence.EventHandler>().As<Persistence.IEventHandler>().InstancePerLifetimeScope();
			builder.Register(c =>
			{
				var options = c.Resolve<IOptions<AppSettings>>();
				var settings = ConnectionSettings.Create()
					.EnableVerboseLogging()
					.UseCustomLogger(c.Resolve<ILogger>())
					.KeepReconnecting()
					.SetReconnectionDelayTo(TimeSpan.FromSeconds(10))
					.Build();
				var connection = EventStoreConnection.Create(settings,
					new Uri($"tcp://{options.Value.EVENTSTORE_HOST}:{options.Value.EVENTSTORE_TCP_PORT}"));
				connection.ConnectAsync().Wait();
				SetupSubscription(c, connection);
				return connection;
			}).As<IEventStoreConnection>().SingleInstance();
			builder.Register(c =>
			{
				var options = c.Resolve<IOptions<AppSettings>>();
				var httpEndpoint = new DnsEndPoint(options.Value.EVENTSTORE_HOST, options.Value.EVENTSTORE_HTTP_PORT);
				var connection = c.Resolve<IEventStoreConnection>();
				return new ProjectionsManager(connection.Settings.Log, httpEndpoint, TimeSpan.FromSeconds(5));
			}).SingleInstance();
		}

		private void SetupSubscription(IComponentContext c, IEventStoreConnection esConnection)
		{

			var freshContext = c.Resolve<IComponentContext>();
			long? lastPosition;
			string stream = "nalozi";
			using (var scope = freshContext.Resolve<ILifetimeScope>().BeginLifetimeScope())
			{
				var processedRepo = scope.Resolve<IProcessedEventRepository>();
				var lastEvent = processedRepo.GetLastProcessed(stream);
				lastPosition = lastEvent?.Checkpoint;
			}
			var options = freshContext.Resolve<IOptions<AppSettings>>();
			var catchUpSettings = new CatchUpSubscriptionSettings(options.Value.EventStoreMaxQueueSize,
				options.Value.EventStoreReadBatchSize, true, true);
			esConnection.SubscribeToStreamFrom(stream, lastPosition, catchUpSettings,
				eventAppeared: async (s, e) =>
				{
					using (var scope = freshContext.Resolve<ILifetimeScope>().BeginLifetimeScope())
					{
						var eventHandler = scope.Resolve<Persistence.IEventHandler>();
						await eventHandler.HandleAsync(e);
					}
				},
				liveProcessingStarted: null,
				subscriptionDropped: (sub, reason, exc) =>
				{
					sub.Stop();
					var conn = freshContext.Resolve<IEventStoreConnection>();
					SetupSubscription(freshContext, conn);
				},
				userCredentials: new UserCredentials(options.Value.EVENTSTORE_USERNAME, options.Value.EVENTSTORE_PASSWORD));
		}
	}
}
