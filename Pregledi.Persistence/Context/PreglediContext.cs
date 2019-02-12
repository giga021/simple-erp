using Microsoft.EntityFrameworkCore;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Seedwork;
using Pregledi.Persistence.EntityConfigurations;
using System.Threading;
using System.Threading.Tasks;

namespace Pregledi.Persistence.Context
{
	public class PreglediContext : DbContext, IUnitOfWork
	{
		public DbSet<NalogGlavnaKnjiga> NaloziGlavneKnjige { get; set; }
		public DbSet<NalogForm> NaloziForm { get; set; }
		public DbSet<StavkaForm> StavkeForm { get; set; }
		public DbSet<KarticaKonta> KarticaKonta { get; set; }
		public DbSet<Konto> Konta { get; set; }
		public DbSet<TipNaloga> TipoviNaloga { get; set; }
		public DbSet<ProcessedEvent> ProcessedEvents { get; set; }

		public PreglediContext(DbContextOptions<PreglediContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new KontoConfiguration());
			modelBuilder.ApplyConfiguration(new TipNalogaConfiguration());
			modelBuilder.ApplyConfiguration(new NalogGlavnaKnjigaConfiguration());
			modelBuilder.ApplyConfiguration(new ProcessedEventConfiguration());
			modelBuilder.ApplyConfiguration(new NalogFormConfiguration());
			modelBuilder.ApplyConfiguration(new StavkaFormConfiguration());
			modelBuilder.ApplyConfiguration(new KarticaKontaConfiguration());
		}

		public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
		{
			await SaveChangesAsync(cancellationToken);
			return true;
		}
	}
}
