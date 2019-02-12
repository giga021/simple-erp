using Autofac;
using ERP.SPA.Application.Consumers;
using ERP.SPA.Infrastructure.Endpoints;
using GreenPipes;
using MassTransit;
using MassTransit.RabbitMqTransport;
using MassTransit.Util;
using Microsoft.Extensions.Options;
using Polly;
using System;
using System.Reflection;

namespace ERP.SPA.Infrastructure.AutofacModules
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

			RegisterSendEndpoints(builder);
		}

		private void RegisterSendEndpoints(ContainerBuilder builder)
		{
			builder.Register<IKnjizenjeSendEndpoint>(c =>
			{
				var options = c.Resolve<IOptions<AppSettings>>();
				var bus = c.Resolve<IBusControl>();
				var endpoint = bus.GetSendEndpoint(new Uri($"rabbitmq://{options.Value.SERVICEBUS_HOST}/knjizenje_queue")).Result;
				return new KnjizenjeSendEndpoint(endpoint);
			}).SingleInstance();
		}

		private void ConfigureHandlers(IRabbitMqBusFactoryConfigurator cfg, IComponentContext context)
		{
			cfg.ReceiveEndpoint("pregledi_queue", e =>
			{
				e.UseRetry(retryConfig =>
				{
					retryConfig.Interval(5, TimeSpan.FromMilliseconds(500));
				});
				e.Consumer<GlavnaKnjigaConsumer>(context);
				e.Consumer<KnjizenjeErrorConsumer>(context);
			});
		}

		private IBusControl CreateAndStartBus(IComponentContext componentContext)
		{
			return busStartPolicy.Execute<IBusControl>(() =>
			{
				var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
				{
					var options = componentContext.Resolve<IOptions<AppSettings>>();
					var hostEndpoint = cfg.Host(new Uri($"rabbitmq://{options.Value.SERVICEBUS_HOST}"), h =>
					{
						h.Username(options.Value.SERVICEBUS_USERNAME);
						h.Password(options.Value.SERVICEBUS_PASSWORD);
					});
					ConfigureHandlers(cfg, componentContext);
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
