using MediatR;
using System;

namespace Pregledi.Application.Events
{
	public class EventBase : INotification
	{
		public Guid EventId { get; set; }
		public long EventNumber { get; set; }
		public DateTime Created { get; set; }
		public string UserId { get; set; }
	}
}
