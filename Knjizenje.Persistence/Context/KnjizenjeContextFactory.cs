using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace Knjizenje.Persistence.Context
{
	public class KnjizenjeContextFactory : IDesignTimeDbContextFactory<KnjizenjeContext>
	{
		public KnjizenjeContext CreateDbContext(string[] args)
		{
			IConfigurationRoot config = new ConfigurationBuilder()
			   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			   .AddJsonFile("appsettings.Migrations.json", optional: false)
			   .Build();

			var optionsBuilder = new DbContextOptionsBuilder<KnjizenjeContext>();
			optionsBuilder.UseMySql(config["ConnectionString"], x => x.ServerVersion(Version.Parse(config["MYSQL_VERSION"]), ServerType.MySql));

			return new KnjizenjeContext(optionsBuilder.Options);
		}
	}
}
