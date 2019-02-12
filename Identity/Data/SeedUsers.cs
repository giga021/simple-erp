using Identity.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Data
{
	public static class SeedUsers
	{
		public static async Task EnsureSeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
		{
			await SeedRolesDataAsync(roleManager);
			await SeedUsersDataAsync(userManager);
		}

		private static async Task SeedUsersDataAsync(UserManager<ApplicationUser> userManager)
		{
			if (await userManager.FindByNameAsync("user1") == null)
			{
				ApplicationUser user = new ApplicationUser();
				user.UserName = "user1";
				user.Email = "user1@localhost";

				IdentityResult result = await userManager.CreateAsync(user, "User.1");

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "NormalUser");
				}
			}

			if (await userManager.FindByNameAsync("admin") == null)
			{
				ApplicationUser user = new ApplicationUser();
				user.UserName = "admin";
				user.Email = "admin@localhost";

				IdentityResult result = await userManager.CreateAsync(user, "Admin.1");

				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "Administrator");
				}
			}
		}

		private static async Task SeedRolesDataAsync(RoleManager<ApplicationRole> roleManager)
		{
			if (!await roleManager.RoleExistsAsync("NormalUser"))
			{
				ApplicationRole role = new ApplicationRole();
				role.Name = "NormalUser";
				IdentityResult roleResult = await roleManager.CreateAsync(role);
			}

			if (!await roleManager.RoleExistsAsync("Administrator"))
			{
				ApplicationRole role = new ApplicationRole();
				role.Name = "Administrator";
				IdentityResult roleResult = await roleManager.CreateAsync(role);
			}
		}
	}
}
