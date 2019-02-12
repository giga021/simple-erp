using Autofac;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;

namespace Pregledi.API.Infrastructure.AutofacModules
{
	public class ServiceBusModule : Autofac.Module
	{
		private Policy busStartPolicy;

		protected override void Load(ContainerBuilder builder)
		{
			busStartPolicy = CreatePolicy();
			builder.Register(c => CreateAndStartBus(c))
				.As<IBusControl>()
				.As<IBus>()
				.SingleInstance()
				.AutoActivate();
		}

		private IBusControl CreateAndStartBus(IComponentContext componentContext)
		{
			return busStartPolicy.Execute<IBusControl>(() =>
			{
				var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
				{
					var loggerFactory = componentContext.Resolve<ILoggerFactory>();
					cfg.UseExtensionsLogging(loggerFactory);

					var options = componentContext.Resolve<IOptions<AppSettings>>();
					var hostEndpoint = cfg.Host(new Uri($"rabbitmq://{options.Value.SERVICEBUS_HOST}"), h =>
					{
						h.Username(options.Value.SERVICEBUS_USERNAME);
						h.Password(options.Value.SERVICEBUS_PASSWORD);
					});
				});
				var busHandle = TaskUtil.Await(() => busControl.StartAsync());
				return busControl;
			});
		}

		private Policy CreatePolicy()
		{
			return Policy.Handle<RabbitMqConnectionException>()
				.WaitAndRetry(10, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
		}
	}
}
