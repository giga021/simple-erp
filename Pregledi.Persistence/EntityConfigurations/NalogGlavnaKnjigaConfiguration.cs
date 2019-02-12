using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pregledi.Domain.Entities;

namespace Pregledi.Persistence.EntityConfigurations
{
	internal class NalogGlavnaKnjigaConfiguration : IEntityTypeConfiguration<NalogGlavnaKnjiga>
	{
		public void Configure(EntityTypeBuilder<NalogGlavnaKnjiga> builder)
		{
			builder.ToTable("nalog_glavna_knjiga");
			builder.Property(x => x.Datum).HasColumnType("date");
			builder.Property(x => x.Version).IsConcurrencyToken();
		}
	}
}
