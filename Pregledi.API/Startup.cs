using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Pregledi.API.Infrastructure.AutofacModules;
using System;

namespace Pregledi.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostingEnvironment currentEnvironment)
		{
			Configuration = configuration;
			CurrentEnvironment = currentEnvironment;
		}

		public IConfiguration Configuration { get; }
		public IHostingEnvironment CurrentEnvironment { get; }

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			var appSettings = new AppSettings();
			Configuration.Bind(appSettings);
			services.Configure<AppSettings>(Configuration);

			services.AddMvc(o =>
			{
				o.Filters.Add(new AuthorizeFilter());
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
			services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
				.AddIdentityServerAuthentication(options =>
				{
					options.Authority = appSettings.IDENTITY_HOST;
					options.RequireHttpsMetadata = false;
					options.ApiName = appSettings.API_NAME;
				});

			GlobalDiagnosticsContext.Set("connectionString", appSettings.ConnectionString);

			var container = new ContainerBuilder();
			container.Populate(services);

			container.RegisterModule(new ApplicationModule());
			container.RegisterModule(new EventStoreModule());
			container.RegisterModule(new MediatrModule());
			container.RegisterModule(new ServiceBusModule());
			return new AutofacServiceProvider(container.Build());
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}
