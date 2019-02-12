using Knjizenje.Domain.Entities.FinNalogAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Knjizenje.Persistence.EntityConfigurations
{
	internal class TipNalogaConfiguration : IEntityTypeConfiguration<TipNaloga>
	{
		public void Configure(EntityTypeBuilder<TipNaloga> builder)
		{
			builder.ToTable("tip_naloga");
		}
	}
}
