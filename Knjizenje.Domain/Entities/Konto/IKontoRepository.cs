using Knjizenje.Domain.Seedwork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knjizenje.Domain.Entities.Konto
{
	public interface IKontoRepository : IRepository<Konto>
	{
		Task<Konto> GetBySifraAsync(string sifra);
		Task<Dictionary<string, long>> GetIdBySifraAsync(params string[] sifra);
	}
}
