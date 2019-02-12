using ERP.SPA.Data.EntityFramework.Configurations;
using ERP.SPA.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ERP.SPA.Data.EntityFramework
{
	public class WebContext : DbContext
	{
		public DbSet<Konto> Konta { get; set; }
		public DbSet<TipNaloga> TipoviNaloga { get; set; }

		public WebContext() { }

		public WebContext(DbContextOptions options) : base(options)
		{
			Database.EnsureCreated();
		}

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
