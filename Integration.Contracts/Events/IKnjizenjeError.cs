using System.Collections.Generic;

namespace Integration.Contracts.Events
{
	public interface IKnjizenjeError
	{
		string Message { get; }
		IEnumerable<IValidationError> ValidationErrors { get; }
	}
}
