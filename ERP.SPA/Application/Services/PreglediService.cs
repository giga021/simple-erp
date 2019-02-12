using ERP.SPA.DTO;
using ERP.SPA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ERP.SPA.Application.Services
{
	public interface IPreglediService
	{
		Task<IList<NalogGKDTO>> GetGlavnaKnjigaAsync();
		Task<NalogFormDTO> GetNalogFormAsync(Guid id);
		Task<IList<KarticaKontaDTO>> GetKarticaKontaAsync(long id);
	}

	public class PreglediService : IPreglediService
	{
		private readonly HttpClient api;
		private readonly IIdentityService identitySvc;

		public PreglediService(HttpClient api, IIdentityService identitySvc)
		{
			this.api = api;
			this.identitySvc = identitySvc;
		}

		public async Task<IList<NalogGKDTO>> GetGlavnaKnjigaAsync()
		{
			using (var requestMessage = CreateRequest(HttpMethod.Get, "api/glavna-knjiga"))
			{
				var response = await api.SendAsync(requestMessage);
				response.EnsureSuccessStatusCode();
				var nalozi = await response.Content.ReadAsAsync<List<NalogGKDTO>>();
				return nalozi;
			}
		}

		public async Task<NalogFormDTO> GetNalogFormAsync(Guid id)
		{
			using (var requestMessage = CreateRequest(HttpMethod.Get, $"api/nalog-form/{id}"))
			{
				var response = await api.SendAsync(requestMessage);
				response.EnsureSuccessStatusCode();
				var nalog = await response.Content.ReadAsAsync<NalogFormDTO>();
				return nalog;
			}
		}

		public async Task<IList<KarticaKontaDTO>> GetKarticaKontaAsync(long id)
		{
			using (var requestMessage = CreateRequest(HttpMethod.Get, $"api/kartica-konta/{id}"))
			{
				var response = await api.SendAsync(requestMessage);
				response.EnsureSuccessStatusCode();
				var kartica = await response.Content.ReadAsAsync<List<KarticaKontaDTO>>();
				return kartica;
			}
		}

		private HttpRequestMessage CreateRequest(HttpMethod method, string url)
		{
			var request = new HttpRequestMessage(method, url);
			request.Headers.Add("Authorization", identitySvc.GetAuthorizationHeader());
			return request;
		}
	}
}
