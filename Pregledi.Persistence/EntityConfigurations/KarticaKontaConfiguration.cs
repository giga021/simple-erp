using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pregledi.Domain.Entities;

namespace Pregledi.Persistence.EntityConfigurations
{
	internal class KarticaKontaConfiguration : IEntityTypeConfiguration<KarticaKonta>
	{
		public void Configure(EntityTypeBuilder<KarticaKonta> builder)
		{
			builder.ToTable("kartica_konta");
			builder.Property(x => x.DatumKnjizenja).HasColumnType("date");
			builder.Property(x => x.DatumNaloga).HasColumnType("date");
			builder.Property(x => x.Konto).HasMaxLength(10).IsRequired();
			builder.Property(x => x.Version).IsConcurrencyToken();
		}
	}
}
