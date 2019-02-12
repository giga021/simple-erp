using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data
{
	public class SeedClients
	{
		public static async Task EnsureSeedDataAsync(PersistedGrantDbContext grantContext, ConfigurationDbContext configContext)
		{
			grantContext.Database.Migrate();
			configContext.Database.Migrate();

			if (!configContext.Clients.Any())
			{
				foreach (var client in Config.GetClients().ToList())
				{
					configContext.Clients.Add(client.ToEntity());
				}
			}

			if (!configContext.IdentityResources.Any())
			{
				foreach (var resource in Config.GetIdentityResources().ToList())
				{
					configContext.IdentityResources.Add(resource.ToEntity());
				}
			}

			if (!configContext.ApiResources.Any())
			{
				foreach (var resource in Config.GetApis().ToList())
				{
					configContext.ApiResources.Add(resource.ToEntity());
				}
			}

			await configContext.SaveChangesAsync();
		}
	}
}
