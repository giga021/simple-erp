using Pregledi.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Pregledi.Application.VersionUpdaters
{
	public class NalogVersionUpdater : IVersionUpdater
	{
		internal const string StreamCategory = "FinNalog";

		private readonly INalogFormRepository formRepo;
		private readonly INalogGKRepository gkRepo;

		public NalogVersionUpdater(INalogFormRepository formRepo, INalogGKRepository gkRepo)
		{
			this.formRepo = formRepo;
			this.gkRepo = gkRepo;
		}

		public async Task UpdateVersionAsync(Guid id, long version)
		{
			var form = await formRepo.GetAsync(id);
			form.Version = version;

			var gk = await gkRepo.GetAsync(id);
			gk.Version = version;
		}
	}
}
