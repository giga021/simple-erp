using Identity.Data;
using Identity.Model;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Identity
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();
			await InitializeDatabaseAsync(host);
			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args)
		{
			return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
		}

		private static async Task InitializeDatabaseAsync(IWebHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
				var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
				await SeedUsers.EnsureSeedDataAsync(userManager, roleManager);
				var grantContext = services.GetRequiredService<PersistedGrantDbContext>();
				var configContext = services.GetRequiredService<ConfigurationDbContext>();
				await SeedClients.EnsureSeedDataAsync(grantContext, configContext);
			}
		}
	}
}
