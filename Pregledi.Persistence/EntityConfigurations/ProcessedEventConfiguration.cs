using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pregledi.Domain.Entities;

namespace Pregledi.Persistence.EntityConfigurations
{
	internal class ProcessedEventConfiguration : IEntityTypeConfiguration<ProcessedEvent>
	{
		public void Configure(EntityTypeBuilder<ProcessedEvent> builder)
		{
			builder.ToTable("processed_event");
			builder.Property(x => x.Stream).HasMaxLength(255).IsRequired();
		}
	}
}
