using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pregledi.Domain.Entities;

namespace Pregledi.Persistence.EntityConfigurations
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
