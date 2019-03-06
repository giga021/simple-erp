using EventStore.ClientAPI;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pregledi.Application;
using Pregledi.Application.Events;
using Pregledi.Domain.Entities;
using Pregledi.Domain.Repositories;
using Pregledi.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pregledi.Persistence
{
	public interface IEventHandler
	{
		Task HandleAsync(ResolvedEvent evnt);
	}

	public class EventHandler : IEventHandler
	{
		private static readonly Lazy<Dictionary<string, Type>> eventTypes = new Lazy<Dictionary<string, Type>>(() =>
			typeof(EventBase).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(EventBase))).ToDictionary(x => x.Name, x => x));

		private readonly ILogger<EventHandler> logger;
		private readonly IMediator mediator;
		private readonly IProcessedEventRepository processedEventRepo;
		private readonly INotificationQueue notifications;
		private readonly IEntityVersionUpdater versionUpdater;
		private readonly IUnitOfWork uow;

		public EventHandler(IMediator mediator, ILogger<EventHandler> logger,
			IProcessedEventRepository processedEventRepo, INotificationQueue notifications,
			IEntityVersionUpdater versionUpdater, IUnitOfWork uow)
		{
			this.mediator = mediator;
			this.logger = logger;
			this.processedEventRepo = processedEventRepo;
			this.notifications = notifications;
			this.versionUpdater = versionUpdater;
			this.uow = uow;
		}

		public async Task HandleAsync(ResolvedEvent evnt)
		{
			if (eventTypes.Value.TryGetValue(evnt.Event.EventType, out var type))
			{
				if (await processedEventRepo.IsProcessedAsync(evnt.Event.EventStreamId, evnt.Event.EventId))
					return;

				try
				{
					var resolvedEvent = DeserializeEvent(evnt, type);
					await mediator.Publish(resolvedEvent);
					await versionUpdater.UpdateVersionAsync(evnt.Event.EventStreamId, evnt.Event.EventNumber);
					await PersistEventAsync(evnt);
					await PublishNotificationsAsync();
				}
				catch (Exception exc)
				{
					logger.LogError(exc, $"Error handling event of type {evnt.Event.EventType} id {evnt.Event.EventId}");
					throw;
				}
			}
		}

		private EventBase DeserializeEvent(ResolvedEvent evnt, Type eventType)
		{
			string dataJson = Encoding.UTF8.GetString(evnt.Event.Data);
			string metadataJson = Encoding.UTF8.GetString(evnt.Event.Metadata);
			var resolvedEvent = (EventBase)JsonConvert.DeserializeObject(dataJson, eventType);
			resolvedEvent.EventNumber = evnt.Event.EventNumber;
			resolvedEvent.EventId = evnt.Event.EventId;
			resolvedEvent.Created = evnt.Event.Created;
			resolvedEvent.UserId = JToken.Parse(metadataJson)["UserId"].Value<string>();
			return resolvedEvent;
		}

		private async Task PersistEventAsync(ResolvedEvent evnt)
		{
			var processed = new ProcessedEvent(evnt.Event.EventId,
				evnt.OriginalStreamId,
				evnt.Event.EventStreamId,
				evnt.OriginalEventNumber,
				evnt.OriginalPosition?.CommitPosition,
				evnt.OriginalPosition?.PreparePosition,
				evnt.Event.Created);
			processedEventRepo.Add(processed);
			await uow.SaveEntitiesAsync();
		}

		private async Task PublishNotificationsAsync()
		{
			foreach (var item in notifications.GetAll())
			{
				await mediator.Publish(item);
			}
		}
	}
}
