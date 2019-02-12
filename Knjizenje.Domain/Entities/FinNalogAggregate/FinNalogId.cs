using Knjizenje.Domain.Seedwork;
using System;
using System.Collections.Generic;

namespace Knjizenje.Domain.Entities.FinNalogAggregate
{
	public class FinNalogId : IAggregateId, IEquatable<FinNalogId>
	{
		private const string StreamPrefix = "FinNalog-";

		public virtual Guid Id { get; }

		public FinNalogId()
		{
			Id = Guid.NewGuid();
		}

		public FinNalogId(Guid id)
		{
			Id = id;
		}

		public FinNalogId(string id)
		{
			Id = Guid.Parse(id.StartsWith(StreamPrefix) ? id.Substring(StreamPrefix.Length) : id);
		}

		public override string ToString()
		{
			return IdAsString();
		}

		public string IdAsString()
		{
			return $"{StreamPrefix}{Id.ToString()}";
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as FinNalogId);
		}

		public bool Equals(FinNalogId other)
		{
			return other != null && Id.Equals(other.Id);
		}

		public override int GetHashCode()
		{
			return 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
		}

		public static bool operator ==(FinNalogId id1, FinNalogId id2)
		{
			return EqualityComparer<FinNalogId>.Default.Equals(id1, id2);
		}

		public static bool operator !=(FinNalogId id1, FinNalogId id2)
		{
			return !(id1 == id2);
		}
	}
}
