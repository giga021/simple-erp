using Autofac;
using Autofac.Extensions.DependencyInjection;
using ERP.SPA.Data.EntityFramework;
using ERP.SPA.Data.Repositories;
using ERP.SPA.Application.Services;
using ERP.SPA.Hubs;
using ERP.SPA.Infrastructure;
using ERP.SPA.Infrastructure.AutofacModules;
using IdentityModel.AspNetCore.OAuth2Introspection;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.Extensions.Http;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Net.Http;

namespace ERP.SPA
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostingEnvironment currentEnvironment)
		{
			Configuration = configuration;
			CurrentEnvironment = currentEnvironment;
			AppSettings = new AppSettings();
			Configuration.Bind(AppSettings);
		}

		public IConfiguration Configuration { get; }
		public IHostingEnvironment CurrentEnvironment { get; }
		public AppSettings AppSettings { get; }

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			services.Configure<AppSettings>(Configuration);

			services.AddMvc(o =>
			{
				o.Filters.Add(new AuthorizeFilter());
			}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
			services.AddSignalR();

			services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
				.AddIdentityServerAuthentication(options =>
				{
					options.Authority = AppSettings.IDENTITY_HOST;
					options.RequireHttpsMetadata = false;
					options.ApiName = AppSettings.API_NAME;
					options.TokenRetriever = new Func<HttpRequest, string>(req =>
					{
						var fromHeader = TokenRetrieval.FromAuthorizationHeader();
						var fromQuery = TokenRetrieval.FromQueryString();
						return fromHeader(req) ?? fromQuery(req);
					});
				});

			services.AddDbContext<WebContext>(options =>
			{
				options.UseMySql(AppSettings.ConnectionString, mySqlOptionsAction: sqlOptions =>
				{
					sqlOptions.ServerVersion(Version.Parse(AppSettings.MYSQL_VERSION), ServerType.MySql);
					sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
				});
			}, ServiceLifetime.Scoped);

			AddApplicationServices(services);
			services.AddSingleton<IUserIdProvider, SignalrUserIdProvider>();

			var container = new ContainerBuilder();
			container.Populate(services);
			container.RegisterModule(new ServiceBusModule());
			return new AutofacServiceProvider(container.Build());
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseCors(builder =>
				{
					builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
				});
			}

			app.UseStaticFiles();
			app.UseCookiePolicy();
			app.UseAuthentication();
			app.UseSignalR(route =>
			{
				route.MapHub<GlavnaKnjigaHub>("/glavna-knjiga-hub");
			});

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				// Catch all Route - catches anything not caught be other routes
				routes.MapRoute(
					name: "catch-all",
					template: "{*url}",
					defaults: new { controller = "Home", action = "Index" }
				);
			});
		}

		private void AddApplicationServices(IServiceCollection services)
		{
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient<IIdentityService, IdentityService>();
			services.AddScoped<IKontoRepository, KontoRepository>();
			services.AddScoped<ITipNalogaRepository, TipNalogaRepository>();
			services.AddScoped<IKnjizenjeService, KnjizenjeService>();
			services.AddHttpClient<IPreglediService, PreglediService>(config =>
			{
				config.BaseAddress = new Uri(AppSettings.PREGLEDI_HOST);
			}).AddPolicyHandler(GetRetryPolicy()).AddPolicyHandler(GetCircuitBreakerPolicy());
		}

		private IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
		{
			return HttpPolicyExtensions.HandleTransientHttpError()
				.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
				.WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
		}

		private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
		{
			return HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
		}
	}
}
