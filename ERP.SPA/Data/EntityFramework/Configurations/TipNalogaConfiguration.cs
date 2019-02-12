using ERP.SPA.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.SPA.Data.EntityFramework.Configurations
{
	internal class TipNalogaConfiguration : IEntityTypeConfiguration<TipNaloga>
	{
		public void Configure(EntityTypeBuilder<TipNaloga> builder)
		{
			builder.ToTable("tip_naloga");

			builder.HasKey(x => x.Id);
			builder.Property(x => x.Id).ValueGeneratedNever();
		}
	}
}
