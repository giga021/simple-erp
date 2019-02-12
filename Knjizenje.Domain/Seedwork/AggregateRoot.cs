using Knjizenje.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Knjizenje.Domain.Seedwork
{
	public class AggregateRoot<TEntity, TKey> : Entity<TKey>, IAggregateRoot
		where TEntity : Entity<TKey>
		where TKey : IAggregateId, IEquatable<TKey>
	{
		private static readonly Lazy<Dictionary<Type, MethodInfo>> applyMethods = new Lazy<Dictionary<Type, MethodInfo>>(InitApplyMethods);
		private static readonly Lazy<ConstructorInfo> constructor = new Lazy<ConstructorInfo>(() =>
			typeof(TEntity).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
			null, new Type[0], new ParameterModifier[0]));

		private bool uninitialized = false;
		private List<EventBase> uncommittedEvents = new List<EventBase>();
		public IReadOnlyCollection<EventBase> UncommittedEvents => uncommittedEvents.AsReadOnly();
		public virtual long Version { get; private set; } = -1;

		protected void Obavesti<TEvent>(TEvent evnt)
			where TEvent : EventBase
		{
			if (uninitialized)
				throw new KnjizenjeException("Aggregate not initialized");
			evnt.EventId = Guid.NewGuid();
			ApplyEvent<TEvent>(evnt);
			uncommittedEvents.Add(evnt);
		}

		public void ApplyEvent<TEvent>(TEvent evnt)
			where TEvent : EventBase
		{
			Type eventType = evnt.GetType();
			if (applyMethods.Value.ContainsKey(eventType))
			{
				var apply = applyMethods.Value[eventType];
				apply.Invoke(this, new[] { evnt });
			}
			if (evnt.EventNumber >= 0)
				Version = evnt.EventNumber;
			uninitialized = false;
		}

		private static Dictionary<Type, MethodInfo> InitApplyMethods()
		{
			return typeof(TEntity).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(x => x.Name == "Primeni" &&
					x.GetParameters().Count() == 1 &&
					x.GetParameters().Single().ParameterType.IsSubclassOf(typeof(EventBase)))
				.ToDictionary(x => x.GetParameters().Single().ParameterType, x => x);
		}

		public void ClearUncommittedEvents()
		{
			uncommittedEvents.Clear();
		}

		public static AggregateRoot<TEntity, TKey> CreateEmpty()
		{
			if (constructor.Value == null)
				throw new InvalidOperationException($"Parameterless constructor for type {typeof(TEntity).Name} not found");

			var emptyAggregate = (AggregateRoot<TEntity, TKey>)constructor.Value.Invoke(new object[0]);
			emptyAggregate.uninitialized = true;
			return emptyAggregate;
		}
	}
}
