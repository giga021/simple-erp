using MediatR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Pregledi.Application
{
	public interface INotificationQueue
	{
		void Add(INotification notification);
		IEnumerable<INotification> GetAll();
	}

	public class NotificationQueue : INotificationQueue
	{
		private readonly HashSet<INotification> notifications;

		public NotificationQueue()
		{
			this.notifications = new HashSet<INotification>();
		}

		public void Add(INotification notification)
		{
			notifications.Add(notification);
		}

		public IEnumerable<INotification> GetAll()
		{
			return notifications.ToList();
		}
	}
}
