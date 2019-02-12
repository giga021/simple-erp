using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pregledi.Domain.Entities;

namespace Pregledi.Persistence.EntityConfigurations
{
	internal class NalogFormConfiguration : IEntityTypeConfiguration<NalogForm>
	{
		public void Configure(EntityTypeBuilder<NalogForm> builder)
		{
			builder.ToTable("nalog_form");
			builder.Property(x => x.Datum).HasColumnType("date");
			builder.Property(x => x.Version).IsConcurrencyToken();
			builder.HasMany(x => x.Stavke).WithOne().HasForeignKey(x => x.IdNaloga).OnDelete(DeleteBehavior.Cascade);
		}
	}
}
