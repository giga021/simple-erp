using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pregledi.Application;
using Pregledi.Application.VersionUpdaters;
using Pregledi.Domain.Repositories;
using Pregledi.Domain.Seedwork;
using Pregledi.Persistence.Context;
using Pregledi.Persistence.Repositories;
using System;

namespace Pregledi.API.Infrastructure.AutofacModules
{
	public class ApplicationModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.Register(c =>
			{
				var options = c.Resolve<IOptions<AppSettings>>();
				var optionsBuilder = new DbContextOptionsBuilder<PreglediContext>();
				optionsBuilder.ConfigureWarnings(x => x.Throw(RelationalEventId.QueryClientEvaluationWarning));
				var env = c.Resolve<IHostingEnvironment>();
				optionsBuilder.EnableSensitiveDataLogging(env.IsDevelopment());
				optionsBuilder.UseMySql(options.Value.ConnectionString, mySqlOptionsAction: sqlOptions =>
				{
					sqlOptions.ServerVersion(Version.Parse(options.Value.MYSQL_VERSION), ServerType.MySql);
					sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
					sqlOptions.MigrationsAssembly(typeof(PreglediContext).Assembly.FullName);
				});
				return optionsBuilder.Options;
			}).SingleInstance();
			builder.RegisterType<PreglediContext>().InstancePerLifetimeScope().AsSelf().As<IUnitOfWork>();
			builder.RegisterType<NotificationQueue>().As<INotificationQueue>().InstancePerLifetimeScope();
			builder.RegisterType<ProcessedEventRepository>().As<IProcessedEventRepository>().InstancePerLifetimeScope();
			builder.RegisterType<EntityVersionUpdater>().As<IEntityVersionUpdater>().InstancePerLifetimeScope();
			builder.RegisterType<EntityVersionUpdaterResolver>().As<IEntityVersionUpdaterResolver>().InstancePerLifetimeScope();
			builder.RegisterType<NalogVersionUpdater>().InstancePerLifetimeScope();
			builder.RegisterType<NalogGKRepository>().As<INalogGKRepository>().InstancePerLifetimeScope();
			builder.RegisterType<TipNalogaRepository>().As<ITipNalogaRepository>().InstancePerLifetimeScope();
			builder.RegisterType<NalogFormRepository>().As<INalogFormRepository>().InstancePerLifetimeScope();
			builder.RegisterType<StavkaFormRepository>().As<IStavkaFormRepository>().InstancePerLifetimeScope();
			builder.RegisterType<KontoRepository>().As<IKontoRepository>().InstancePerLifetimeScope();
			builder.RegisterType<KarticaKontaRepository>().As<IKarticaKontaRepository>().InstancePerLifetimeScope();
		}
	}
}
