using Autofac;
using FluentValidation;
using GreenPipes;
using Knjizenje.API.Infrastructure.ServiceBus;
using Knjizenje.Domain.Exceptions;
using Knjizenje.Service;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Reflection;

namespace Knjizenje.API.Infrastructure.AutofacModules
{
	public class ServiceBusModule : Autofac.Module
	{
		private Policy busStartPolicy;

		protected override void Load(ContainerBuilder builder)
		{
			busStartPolicy = CreatePolicy();
			builder.RegisterConsumers(Assembly.GetExecutingAssembly());
			builder.Register(c => CreateAndStartBus(c))
				.As<IBusControl>()
				.As<IBus>()
				.SingleInstance()
				.AutoActivate();
		}

		private void ConfigureHandlers(IRabbitMqHost host, IRabbitMqBusFactoryConfigurator cfg,
			IComponentContext context)
		{
			cfg.ReceiveEndpoint(host, "knjizenje_queue", e =>
			{
				e.UseRetry(retryConfig =>
				{
					retryConfig.Interval(5, TimeSpan.FromMilliseconds(500));
					retryConfig.Ignore<ValidationException>();
					retryConfig.Ignore<KnjizenjeException>();
				});
				e.LoadFrom(context);
			});
		}

		private IBusControl CreateAndStartBus(IComponentContext componentContext)
		{
			return busStartPolicy.Execute<IBusControl>(() =>
			{
				var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
				{
					var loggerFactory = componentContext.Resolve<ILoggerFactory>();
					cfg.UseExtensionsLogging(loggerFactory);
					cfg.UseConsumeExceptionHandler();

					var options = componentContext.Resolve<IOptions<AppSettings>>();
					var hostEndpoint = cfg.Host(new Uri($"rabbitmq://{options.Value.SERVICEBUS_HOST}"), h =>
					{
						h.Username(options.Value.SERVICEBUS_USERNAME);
						h.Password(options.Value.SERVICEBUS_PASSWORD);
					});
					ConfigureHandlers(hostEndpoint, cfg, componentContext);
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
