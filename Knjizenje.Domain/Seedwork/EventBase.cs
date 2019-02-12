using System;
using System.Collections.Generic;

namespace Knjizenje.Domain.Seedwork
{
	public class EventBase : IEquatable<EventBase>
	{
		public Guid EventId { get; set; }
		public long EventNumber { get; set; } = -1;

		public override bool Equals(object obj)
		{
			return Equals(obj as EventBase);
		}

		public bool Equals(EventBase other)
		{
			return other != null &&
				   EventId.Equals(other.EventId) &&
				   EventNumber == other.EventNumber;
		}

		public override int GetHashCode()
		{
			var hashCode = -1775069120;
			hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(EventId);
			hashCode = hashCode * -1521134295 + EventNumber.GetHashCode();
			return hashCode;
		}

		public static bool operator ==(EventBase base1, EventBase base2)
		{
			return EqualityComparer<EventBase>.Default.Equals(base1, base2);
		}

		public static bool operator !=(EventBase base1, EventBase base2)
		{
			return !(base1 == base2);
		}
	}
}
