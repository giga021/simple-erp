using Integration.Contracts.Events;
using MediatR;
using System;

namespace Pregledi.Application.Notifications
{
	public class GlavnaKnjigaChanged : INotification, IGlavnaKnjigaChanged
	{
		public string UserId { get; }

		public GlavnaKnjigaChanged(string userId)
		{
			this.UserId = userId;
		}

		public override bool Equals(object obj)
		{
			var changed = obj as GlavnaKnjigaChanged;
			return changed != null && UserId == changed.UserId;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(UserId);
		}
	}
}
