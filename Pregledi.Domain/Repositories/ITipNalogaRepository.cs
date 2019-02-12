using Pregledi.Domain.Entities;
using System.Threading.Tasks;

namespace Pregledi.Domain.Repositories
{
	public interface ITipNalogaRepository
	{
		Task<TipNaloga> GetAsync(long id);
	}
}
