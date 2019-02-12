using Knjizenje.Domain.Seedwork;
using Newtonsoft.Json;
using System;

namespace Knjizenje.Persistence.EventSourcing.Serialization
{
	public class AggregateIdConverter : JsonConverter<IAggregateId>
	{
		public override void WriteJson(JsonWriter writer, IAggregateId value, JsonSerializer serializer)
		{
			writer.WriteValue(value.Id.ToString());
		}

		public override IAggregateId ReadJson(JsonReader reader, Type objectType, IAggregateId existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			string fullId = reader.Value.ToString();
			var idAggregate = objectType.GetConstructor(new[] { typeof(string) }).Invoke(new[] { fullId });
			return (IAggregateId)idAggregate;
		}
	}
}
