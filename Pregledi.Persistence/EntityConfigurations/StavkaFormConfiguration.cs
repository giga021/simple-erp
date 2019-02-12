using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pregledi.Domain.Entities;

namespace Pregledi.Persistence.EntityConfigurations
{
	internal class StavkaFormConfiguration : IEntityTypeConfiguration<StavkaForm>
	{
		public void Configure(EntityTypeBuilder<StavkaForm> builder)
		{
			builder.ToTable("stavka_form");
			builder.Property(x => x.DatumKnjizenja).HasColumnType("date");
			builder.Property(x => x.Konto).HasMaxLength(10).IsRequired();
			builder.Property(x => x.Version).IsConcurrencyToken();
		}
	}
}
