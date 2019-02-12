using Integration.Contracts.Events;

namespace Knjizenje.API.Infrastructure.ServiceBus.ErrorHandling.ErrorModel
{
	public class ValidationError : IValidationError
	{
		public string Property { get; }
		public string Message { get; }

		public ValidationError(string property, string message)
		{
			this.Property = property;
			this.Message = message;
		}
	}
}
