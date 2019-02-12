using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Entities.Konto;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Knjizenje.Persistence.Context
{
	public class KnjizenjeContextSeed
	{
		public async Task SeedAsync(KnjizenjeContext context)
		{
			context.Database.Migrate();

			if (!context.TipoviNaloga.Any())
			{
				context.TipoviNaloga.AddRange(new[]
				{
					TipNaloga.UlazneFakture,
					TipNaloga.Izvodi
				});
			}
			if (!context.Konta.Any())
			{
				context.Konta.AddRange(new[]
				{
					new Konto("435", "Zaduzenje"),
					new Konto("241", "Tekuci racun")
				});
			}

			await context.SaveChangesAsync();
		}
	}
}
