using Microsoft.AspNetCore.SignalR;

namespace ERP.SPA.Infrastructure
{
	public class SignalrUserIdProvider : IUserIdProvider
	{
		public string GetUserId(HubConnectionContext connection)
		{
			return connection.User.FindFirst("sub")?.Value;
		}
	}
}
