// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace Identity
{
	public static class Config
	{
		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new IdentityResource[]
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
			};
		}

		public static IEnumerable<ApiResource> GetApis()
		{
			return new ApiResource[]
			{
				new ApiResource("knjizenje", "Knjiženje")
				{
					UserClaims = { ClaimTypes.Role }
				}
			};
		}

		public static IEnumerable<Client> GetClients()
		{
			return new[]
			{
				new Client
				{
					ClientId = "js",
					ClientName = "JS Client",
					AllowedGrantTypes = GrantTypes.Implicit,
					AllowAccessTokensViaBrowser = true,
					AccessTokenLifetime = 120000,
					RedirectUris =
					{
						"http://localhost:3000/callback",
						"http://localhost:3000/silent-renew",
						"http://localhost:8081/callback",
						"http://localhost:8081/silent-renew"
					},
					RequireConsent = false,
					PostLogoutRedirectUris =
					{
						"http://localhost:3000/",
						"http://localhost:8081/"
					},
					AllowedCorsOrigins = {
						"http://localhost:3000",
						"http://localhost:8081"
					},
					AllowedScopes =
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"knjizenje",
					}
				}
			};
		}
	}
}
