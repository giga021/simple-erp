using System;
using System.Threading.Tasks;

namespace Pregledi.Application.VersionUpdaters
{
	public interface IVersionUpdater
	{
		Task UpdateVersionAsync(Guid id, long version);
	}
}
