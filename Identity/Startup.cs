using Identity.Data;
using Identity.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace Identity
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public IHostingEnvironment CurrentEnvironment { get; }

		public Startup(IConfiguration configuration, IHostingEnvironment currentEnvironment)
		{
			Configuration = configuration;
			CurrentEnvironment = currentEnvironment;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			var appSettings = new AppSettings();
			Configuration.Bind(appSettings);
			services.Configure<AppSettings>(Configuration);
			services.AddMvc();

			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseMySql(appSettings.ConnectionStringUsers, mySqlOptionsAction: sqlOptions =>
				{
					sqlOptions.ServerVersion(Version.Parse(appSettings.MYSQL_VERSION), ServerType.MySql);
					sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
					sqlOptions.MigrationsAssembly(typeof(SeedClients).Assembly.FullName);
				});
			}, ServiceLifetime.Scoped);

			services.AddIdentity<ApplicationUser, ApplicationRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddIdentityServer()
				.AddDeveloperSigningCredential()
				.AddConfigurationStore(options =>
				{
					options.ConfigureDbContext = builder =>
						builder.UseMySql(appSettings.ConnectionStringClients, mySqlOptionsAction: sqlOptions =>
						{
							sqlOptions.ServerVersion(Version.Parse(appSettings.MYSQL_VERSION), ServerType.MySql);
							sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
							sqlOptions.MigrationsAssembly(typeof(SeedClients).Assembly.FullName);
						});
				})
				.AddOperationalStore(options =>
				{
					options.ConfigureDbContext = builder =>
						builder.UseMySql(appSettings.ConnectionStringGrants, mySqlOptionsAction: sqlOptions =>
						{
							sqlOptions.ServerVersion(Version.Parse(appSettings.MYSQL_VERSION), ServerType.MySql);
							sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
							sqlOptions.MigrationsAssembly(typeof(SeedClients).Assembly.FullName);
						});
				})
				.AddAspNetIdentity<ApplicationUser>();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors(builder =>
			{
				builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
			});

			app.UseIdentityServer();
			app.UseStaticFiles();
			app.UseMvcWithDefaultRoute();
		}
	}
}
