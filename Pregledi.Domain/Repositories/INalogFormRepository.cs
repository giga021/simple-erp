using Pregledi.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Pregledi.Domain.Repositories
{
	public interface INalogFormRepository
	{
		void Add(NalogForm nalog);
		Task<NalogForm> GetAsync(Guid id);
		Task<NalogForm> GetSaStavkamaAsync(Guid id);
		void Remove(NalogForm nalog);
	}
}
