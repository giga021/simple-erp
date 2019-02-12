using Pregledi.Domain.Entities;
using System.Threading.Tasks;

namespace Pregledi.Domain.Repositories
{
	public interface IKontoRepository
	{
		Task<Konto> GetAsync(long id);
	}
}
