using FluentValidation;
using Knjizenje.Application.Commands;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.API.Application.Behaviors
{
	public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
		where TRequest : ICommand
	{
		private readonly IValidator<TRequest>[] _validators;

		public ValidatorBehavior(IValidator<TRequest>[] validators)
		{
			_validators = validators;
		}

		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
			RequestHandlerDelegate<TResponse> next)
		{
			var failures = _validators
				.Select(v => v.Validate(request))
				.SelectMany(result => result.Errors)
				.Where(error => error != null)
				.ToList();

			if (failures.Any())
			{
				throw new ValidationException("Komanda nije validna", failures);
			}

			var response = await next();
			return response;
		}
	}
}
