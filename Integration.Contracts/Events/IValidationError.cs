namespace Integration.Contracts.Events
{
	public interface IValidationError
	{
		string Property { get; }
		string Message { get; }
	}
}
