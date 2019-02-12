namespace ERP.SPA.Infrastructure
{
	public interface IIdentityService
	{
		string GetUserId();

		string GetUserName();

		string GetAuthorizationHeader();
	}
}
