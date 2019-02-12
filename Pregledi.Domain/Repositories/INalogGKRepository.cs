using Pregledi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pregledi.Domain.Repositories
{
	public interface INalogGKRepository
	{
		void Add(NalogGlavnaKnjiga nalog);
		Task<NalogGlavnaKnjiga> GetAsync(Guid id);
		Task<IList<NalogGlavnaKnjiga>> GetAllAsync();
		void Remove(NalogGlavnaKnjiga nalog);
	}
}
