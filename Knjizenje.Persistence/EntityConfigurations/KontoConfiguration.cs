using Knjizenje.Domain.Entities.Konto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Knjizenje.Persistence.EntityConfigurations
{
	internal class KontoConfiguration : IEntityTypeConfiguration<Konto>
	{
		public void Configure(EntityTypeBuilder<Konto> builder)
		{
			builder.ToTable("konto");
		}
	}
}
