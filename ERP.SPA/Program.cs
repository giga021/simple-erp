using ERP.SPA.Data.EntityFramework;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace ERP.SPA
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

		public static async Task InitializeDatabaseAsync(IWebHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				using (var context = services.GetRequiredService<WebContext>())
				{
					var seeder = new WebContextSeed();
					await seeder.SeedAsync(context);
				}
			}
		}
	}
}
