using System;

namespace Knjizenje.Domain.Seedwork
{
	public interface IAggregateId
	{
		string IdAsString();
		Guid Id { get; }
	}
}
