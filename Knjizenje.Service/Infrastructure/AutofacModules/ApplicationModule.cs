using Autofac;
using Knjizenje.Domain.Entities.FinNalogAggregate;
using Knjizenje.Domain.Entities.Konto;
using Knjizenje.Domain.Services;
using Knjizenje.Persistence.Context;
using Knjizenje.Persistence.Repositories;
using Knjizenje.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace Knjizenje.API.Infrastructure.AutofacModules
{
	public class ApplicationModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c =>
			{
				var options = c.Resolve<IOptions<AppSettings>>();
				var optionsBuilder = new DbContextOptionsBuilder<KnjizenjeContext>();
				optionsBuilder.ConfigureWarnings(x => x.Throw(RelationalEventId.QueryClientEvaluationWarning));
				var env = c.Resolve<IHostingEnvironment>();
				optionsBuilder.EnableSensitiveDataLogging(env.IsDevelopment());
				optionsBuilder.UseMySql(options.Value.ConnectionString, mySqlOptionsAction: sqlOptions =>
				{
					sqlOptions.ServerVersion(Version.Parse(options.Value.MYSQL_VERSION), ServerType.MySql);
					sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
					sqlOptions.MigrationsAssembly(typeof(KnjizenjeContext).Assembly.FullName);
				});
				return optionsBuilder.Options;
			}).SingleInstance();
			builder.RegisterType<KnjizenjeContext>();
			builder.RegisterType<FinNalogRepository>().As<IFinNalogRepository>();
			builder.RegisterType<KontoRepository>().As<IKontoRepository>();
			builder.RegisterType<IzvodService>().As<IIzvodService>();
			builder.RegisterType<FinNalogService>().As<IFinNalogService>();
		}
	}
}
