using FluentValidation;
using GreenPipes;
using Knjizenje.API.Infrastructure.ServiceBus.ErrorHandling.ErrorModel;
using Knjizenje.Domain.Exceptions;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace Knjizenje.API.Infrastructure.ServiceBus.ErrorHandling
{
	public class ConsumeExceptionHandlerFilter<T> : IFilter<T>
		where T : class, ConsumeContext
	{
		public void Probe(ProbeContext context)
		{
			var scope = context.CreateFilterScope("knjizenjeExceptionHandler");
		}

		public async Task Send(T context, IPipe<T> next)
		{
			try
			{
				await next.Send(context);
			}
			catch (ValidationException exc)
			{
				var error = new KnjizenjeError(exc.Message,
					exc.Errors.Select(e => new ValidationError(e.PropertyName, e.ErrorMessage)));
				await context.Publish(error);
				throw;
			}
			catch (KnjizenjeException exc)
			{
				var error = new KnjizenjeError(exc.Message);
				await context.Publish(error);
				throw;
			}
		}
	}
}
