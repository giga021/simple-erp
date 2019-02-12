using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pregledi.Domain.Entities;

namespace Pregledi.Persistence.EntityConfigurations
{
	internal class KontoConfiguration : IEntityTypeConfiguration<Konto>
	{
		public void Configure(EntityTypeBuilder<Konto> builder)
		{
			builder.ToTable("konto");
		}
	}
}
