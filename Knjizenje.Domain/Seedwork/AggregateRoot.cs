using Knjizenje.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Knjizenje.Domain.Seedwork
{
	public class AggregateRoot<TEntity, TKey> : Entity<TKey>, IAggregateRoot
		where TEntity : Entity<TKey>
		where TKey : IAggregateId, IEquatable<TKey>
	{
		private static readonly Lazy<Dictionary<Type, Action<AggregateRoot<TEntity, TKey>, EventBase>>> applyMethods = new Lazy<Dictionary<Type, Action<AggregateRoot<TEntity, TKey>, EventBase>>>(InitApplyMethods);
		private static readonly Lazy<Func<AggregateRoot<TEntity, TKey>>> constructor = new Lazy<Func<AggregateRoot<TEntity, TKey>>>(() =>
			Expression.Lambda<Func<AggregateRoot<TEntity, TKey>>>(Expression.New(typeof(TEntity))).Compile());

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
			if (applyMethods.Value.TryGetValue(eventType, out var apply))
				apply.Invoke(this, evnt);

			if (evnt.EventNumber >= 0)
				Version = evnt.EventNumber;

			uninitialized = false;
		}

		private static Dictionary<Type, Action<AggregateRoot<TEntity, TKey>, EventBase>> InitApplyMethods()
		{
			var aggregateParamBase = Expression.Parameter(typeof(AggregateRoot<TEntity, TKey>), "agg");
			var evntParamBase = Expression.Parameter(typeof(EventBase), "evnt");
			var methods = typeof(TEntity).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(x => x.Name == "Primeni" &&
					x.GetParameters().Count() == 1 &&
					x.GetParameters().Single().ParameterType.IsSubclassOf(typeof(EventBase)));

			var compiledMethods = new Dictionary<Type, Action<AggregateRoot<TEntity, TKey>, EventBase>>();
			foreach (var item in methods)
			{
				var eventType = item.GetParameters().Single().ParameterType;
				var doApply = Expression.Call(Expression.Convert(aggregateParamBase, typeof(TEntity)), item, Expression.Convert(evntParamBase, eventType));
				compiledMethods[eventType] = Expression.Lambda<Action<AggregateRoot<TEntity, TKey>, EventBase>>(doApply, aggregateParamBase, evntParamBase).Compile();
			}
			return compiledMethods;
		}

		public void ClearUncommittedEvents()
		{
			uncommittedEvents.Clear();
		}

		public static AggregateRoot<TEntity, TKey> CreateEmpty()
		{
			var emptyAggregate = constructor.Value.Invoke();
			emptyAggregate.uninitialized = true;
			return emptyAggregate;
		}
	}
}
