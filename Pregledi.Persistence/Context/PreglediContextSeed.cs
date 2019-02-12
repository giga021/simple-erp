using Microsoft.EntityFrameworkCore;
using Pregledi.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Pregledi.Persistence.Context
{
	public class PreglediContextSeed
	{
		public async Task SeedAsync(PreglediContext context)
		{
			context.Database.Migrate();

			if (!context.TipoviNaloga.Any())
			{
				context.TipoviNaloga.AddRange(new[]
				{
					new TipNaloga() { Id = 1, Naziv = "Ulazne fakture" },
					new TipNaloga() { Id = 2, Naziv = "Izvodi" },
				});
			}
			if (!context.Konta.Any())
			{
				context.Konta.AddRange(new[]
				{
					new Konto() { Sifra = "435", Naziv = "Zaduzenje" },
					new Konto() { Sifra = "241", Naziv = "Tekuci racun" },
				});
			}

			await context.SaveChangesAsync();
		}
	}
}
