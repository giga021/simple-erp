using ERP.SPA.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ERP.SPA.Data.EntityFramework.Configurations
{
	internal class KontoConfiguration : IEntityTypeConfiguration<Konto>
	{
		public void Configure(EntityTypeBuilder<Konto> builder)
		{
			builder.ToTable("konto");
		}
	}
}
