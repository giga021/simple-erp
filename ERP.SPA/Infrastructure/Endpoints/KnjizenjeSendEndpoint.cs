using MassTransit;

namespace ERP.SPA.Infrastructure.Endpoints
{
	public interface IKnjizenjeSendEndpoint
	{
		ISendEndpoint Endpoint { get; }
	}

	public class KnjizenjeSendEndpoint : IKnjizenjeSendEndpoint
	{
		public ISendEndpoint Endpoint { get; }

		public KnjizenjeSendEndpoint(ISendEndpoint endpoint)
		{
			this.Endpoint = endpoint;
		}
	}
}
