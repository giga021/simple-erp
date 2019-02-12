using System;
using System.Threading.Tasks;

namespace Pregledi.Application
{
	public interface IEntityVersionUpdater
	{
		Task UpdateVersionAsync(string stream, long version);
	}

	public class EntityVersionUpdater : IEntityVersionUpdater
	{
		private readonly IEntityVersionUpdaterResolver resolver;

		public EntityVersionUpdater(IEntityVersionUpdaterResolver resolver)
		{
			this.resolver = resolver;
		}

		public async Task UpdateVersionAsync(string stream, long version)
		{
			var parsed = ParseStream(stream);
			var updater = resolver.Resolve(parsed.Category);
			await updater.UpdateVersionAsync(parsed.Id, version);
		}

		private (string Category, Guid Id) ParseStream(string stream)
		{
			if (stream == null)
				throw new ArgumentNullException(stream);

			int splitterIdx = stream.IndexOf('-');
			if (splitterIdx > 0)
			{
				string category = stream.Substring(0, splitterIdx);
				int idIdx = splitterIdx + 1;
				string id = stream.Substring(idIdx, stream.Length - idIdx);
				return (Category: category, Id: Guid.Parse(id));
			}
			else
			{
				throw new ArgumentException($"Unable to parse stream '{stream}'", nameof(stream));
			}
		}
	}
}
