using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Knjizenje.Domain.Seedwork
{
	public abstract class Enumeration<T> : IComparable
		where T : Enumeration<T>
	{
		public string Name { get; private set; }

		public long Id { get; private set; }

		protected Enumeration() { }

		protected Enumeration(long id, string name)
		{
			Id = id;
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}

		public static IEnumerable<T> GetAll()
		{
			var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

			return fields.Select(f => f.GetValue(null)).OfType<T>();
		}

		public static T Get(long id)
		{
			return GetAll().SingleOrDefault(x => x.Id == id);
		}

		public override bool Equals(object obj)
		{
			var otherValue = obj as Enumeration<T>;

			if (otherValue == null)
				return false;

			var typeMatches = GetType().Equals(obj.GetType());
			var valueMatches = Id.Equals(otherValue.Id);

			return typeMatches && valueMatches;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public int CompareTo(object other)
		{
			return Id.CompareTo(((Enumeration<T>)other).Id);
		}
	}
}
