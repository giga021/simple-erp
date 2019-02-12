using Microsoft.Extensions.DependencyInjection;
using Pregledi.Application.VersionUpdaters;
using System;

namespace Pregledi.Application
{
	public interface IEntityVersionUpdaterResolver
	{
		IVersionUpdater Resolve(string category);
	}

	public class EntityVersionUpdaterResolver : IEntityVersionUpdaterResolver
	{
		private readonly IServiceProvider serviceProvider;

		public EntityVersionUpdaterResolver(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public IVersionUpdater Resolve(string category)
		{
			switch (category)
			{
				case NalogVersionUpdater.StreamCategory:
					return serviceProvider.GetRequiredService<NalogVersionUpdater>();
				default:
					throw new ArgumentException($"Unknown version updater for '{category}'");
			}
		}
	}
}
