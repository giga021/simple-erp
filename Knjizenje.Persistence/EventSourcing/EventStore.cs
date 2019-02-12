using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using EventStore.ClientAPI.Projections;
using Knjizenje.Domain.Exceptions;
using Knjizenje.Domain.Seedwork;
using Knjizenje.Persistence.EventSourcing.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Knjizenje.Persistence.EventSourcing
{
	public interface IEventStore
	{
		Task<IList<EventBase>> ReadEventsAsync(string stream);
		Task WriteEventsAsync(string stream, IEnumerable<EventBase> events, long? expectedVersion, Guid commandId, string userId);
		Task<IList<T>> GetProjectionAsync<T>(string projection);
	}

	public class EventStore : IEventStore
	{
		private static readonly Lazy<Dictionary<string, Type>> eventTypes = new Lazy<Dictionary<string, Type>>(() =>
			typeof(EventBase).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(EventBase))).ToDictionary(x => x.Name, x => x));

		private readonly IEventStoreConnection connection;
		private readonly ProjectionsManager projectionManager;

		public EventStore(IEventStoreConnection connection, ProjectionsManager projectionManager)
		{
			this.connection = connection;
			this.projectionManager = projectionManager;
		}

		public async Task<IList<T>> GetProjectionAsync<T>(string projection)
		{
			if (projection == null)
				throw new ArgumentNullException(nameof(projection));

			var state = await projectionManager.GetStateAsync(projection);
			if (string.IsNullOrEmpty(state))
				return Enumerable.Empty<T>().ToList();
			var eventSerializer = new EventSerializerContractResolver();
			var jsonSettings = new JsonSerializerSettings()
			{
				ContractResolver = eventSerializer
			};
			var stateObjects = JsonConvert.DeserializeObject<List<T>>(state, jsonSettings);
			return stateObjects;
		}

		public async Task<IList<EventBase>> ReadEventsAsync(string stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			var ret = new List<EventBase>();
			StreamEventsSlice currentSlice;
			long nextSliceStart = StreamPosition.Start;
			var eventSerializer = new EventSerializerContractResolver();
			var jsonSettings = new JsonSerializerSettings()
			{
				ContractResolver = eventSerializer
			};
			do
			{
				currentSlice = await connection.ReadStreamEventsForwardAsync(stream, nextSliceStart, 1000, false);
				if (currentSlice.Status != SliceReadStatus.Success)
				{
					throw new InvalidOperationException($"Error reading stream '{stream}'");
				}
				nextSliceStart = currentSlice.NextEventNumber;
				foreach (var resolvedEvent in currentSlice.Events)
				{
					string json = Encoding.UTF8.GetString(resolvedEvent.Event.Data);
					Type eventType = eventTypes.Value[resolvedEvent.Event.EventType];
					//Type eventType = Type.GetType(resolvedEvent.Event.EventType);
					var evnt = (EventBase)JsonConvert.DeserializeObject(json, eventType, jsonSettings);
					evnt.EventNumber = resolvedEvent.Event.EventNumber;
					evnt.EventId = resolvedEvent.Event.EventId;
					ret.Add(evnt);
				}
			}
			while (!currentSlice.IsEndOfStream);
			return ret;
		}

		public async Task WriteEventsAsync(string stream, IEnumerable<EventBase> events, long? expectedVersion, Guid commandId, string userId)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));
			if (events == null)
				throw new ArgumentNullException(nameof(events));

			if (expectedVersion == null)
				expectedVersion = ExpectedVersion.NoStream;

			var eventSerializer = new EventSerializerContractResolver();
			var eventJsonSettings = new JsonSerializerSettings()
			{
				ContractResolver = eventSerializer
			};
			var eventMetadata = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new EventMetadata(commandId, userId)));
			var eventData = events.Select(x =>
			{
				string json = JsonConvert.SerializeObject(x, eventJsonSettings);
				var serializedEvent = Encoding.UTF8.GetBytes(json);

				return new EventData(x.EventId, x.GetType().Name, true, serializedEvent, eventMetadata);
			});
			try
			{
				await connection.AppendToStreamAsync(stream, expectedVersion.Value, eventData);
			}
			catch (WrongExpectedVersionException exc)
			{
				throw new KnjizenjeException("Slog je promenjen ili obrisan u međuvremenu", exc);
			}
		}
	}
}
