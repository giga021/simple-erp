using GreenPipes;
using Knjizenje.API.Infrastructure.ServiceBus.ErrorHandling;
using MassTransit;

namespace Knjizenje.API.Infrastructure.ServiceBus
{
	public static class ServiceBusExtensions
	{
		public static void UseConsumeExceptionHandler<T>(this IPipeConfigurator<T> configurator)
			where T : class, ConsumeContext
		{
			configurator.AddPipeSpecification(new ConsumeExceptionHandlerSpecification<T>());
		}
	}
}
