using Autofac;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Projections;
using Knjizenje.Service;
using Knjizenje.Service.Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using ES = Knjizenje.Persistence.EventSourcing;

namespace Knjizenje.API.Infrastructure.AutofacModules
{
	public class EventStoreModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<EventStoreLogger>().As<ILogger>();
			builder.Register(c =>
			{
				var options = c.Resolve<IOptions<AppSettings>>();
				var settings = ConnectionSettings.Create()
					.UseCustomLogger(c.Resolve<ILogger>())
					.Build();
				var connection = EventStoreConnection.Create(settings,
					new Uri($"tcp://{options.Value.EVENTSTORE_HOST}:{options.Value.EVENTSTORE_TCP_PORT}"));
				connection.ConnectAsync().Wait();
				return connection;
			}).As<IEventStoreConnection>().SingleInstance();
			builder.Register(c =>
			{
				var options = c.Resolve<IOptions<AppSettings>>();
				var httpEndpoint = new DnsEndPoint(options.Value.EVENTSTORE_HOST, options.Value.EVENTSTORE_HTTP_PORT);
				var connection = c.Resolve<IEventStoreConnection>();
				return new ProjectionsManager(connection.Settings.Log, httpEndpoint, TimeSpan.FromSeconds(5));
			}).SingleInstance();
			builder.RegisterType<ES.EventStore>().As<ES.IEventStore>();
		}
	}
}
