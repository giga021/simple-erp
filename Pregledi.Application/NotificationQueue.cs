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
		private readonly ConcurrentDictionary<INotification, INotification> notifications;

		public NotificationQueue()
		{
			this.notifications = new ConcurrentDictionary<INotification, INotification>();
		}

		public void Add(INotification notification)
		{
			notifications.AddOrUpdate(notification, notification, (k, v) => notification);
		}

		public IEnumerable<INotification> GetAll()
		{
			return notifications.Select(x => x.Key).ToList();
		}
	}
}
