namespace Pregledi.Domain.Seedwork
{
	using System;

	public abstract class Entity<TKey>
		where TKey : IEquatable<TKey>
	{
		private TKey _id;
		public virtual TKey Id
		{
			get => _id;
			set => _id = value;
		}

		public bool IsTransient()
		{
			return this.Id.Equals(default(TKey));
		}

		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is Entity<TKey>))
				return false;

			if (Object.ReferenceEquals(this, obj))
				return true;

			if (this.GetType() != obj.GetType())
				return false;

			Entity<TKey> item = (Entity<TKey>)obj;

			if (item.IsTransient() || this.IsTransient())
				return false;
			else
				return item.Id.Equals(this.Id);
		}

		public override int GetHashCode()
		{
			if (!IsTransient())
				return this.Id.GetHashCode() ^ 31;
			else
				return base.GetHashCode();
		}

		public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
		{
			if (Object.Equals(left, null))
				return (Object.Equals(right, null)) ? true : false;
			else
				return left.Equals(right);
		}

		public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
		{
			return !(left == right);
		}
	}
}
