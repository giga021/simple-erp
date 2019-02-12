using GreenPipes;
using MassTransit;
using System.Collections.Generic;
using System.Linq;

namespace Knjizenje.API.Infrastructure.ServiceBus.ErrorHandling
{
	public class ConsumeExceptionHandlerSpecification<T> : IPipeSpecification<T>
		where T : class, ConsumeContext
	{
		public IEnumerable<ValidationResult> Validate()
		{
			return Enumerable.Empty<ValidationResult>();
		}

		public void Apply(IPipeBuilder<T> builder)
		{
			builder.AddFilter(new ConsumeExceptionHandlerFilter<T>());
		}
	}
}
