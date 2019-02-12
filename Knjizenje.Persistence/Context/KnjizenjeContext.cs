using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Entities.Konto;
using Knjizenje.Domain.Seedwork;
using Knjizenje.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Knjizenje.Persistence.Context
{
	public class KnjizenjeContext : DbContext, IUnitOfWork
	{
		public DbSet<TipNaloga> TipoviNaloga { get; set; }
		public DbSet<Konto> Konta { get; set; }

		public KnjizenjeContext(DbContextOptions<KnjizenjeContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new KontoConfiguration());
			modelBuilder.ApplyConfiguration(new TipNalogaConfiguration());
		}

		public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			await SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
