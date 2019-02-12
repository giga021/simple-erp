using Integration.Contracts.Events;
using System.Collections.Generic;

namespace Knjizenje.API.Infrastructure.ServiceBus.ErrorHandling.ErrorModel
{
	public class KnjizenjeError : IKnjizenjeError
	{
		public string Message { get; }
		public IEnumerable<IValidationError> ValidationErrors { get; }

		public KnjizenjeError(string message)
		{
			this.Message = message;
		}

		public KnjizenjeError(string message, IEnumerable<ValidationError> validationErrors)
			: this(message)
		{
			this.ValidationErrors = validationErrors;
		}
	}
}
