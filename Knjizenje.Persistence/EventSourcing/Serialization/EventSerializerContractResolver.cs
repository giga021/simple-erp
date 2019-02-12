using Knjizenje.Domain.Seedwork;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Reflection;

namespace Knjizenje.Persistence.EventSourcing.Serialization
{
	public class EventSerializerContractResolver : DefaultContractResolver
	{
		protected override JsonProperty CreateProperty(MemberInfo member,
										 MemberSerialization memberSerialization)
		{
			JsonProperty property = base.CreateProperty(member, memberSerialization);

			if (property.DeclaringType.IsSubclassOf(typeof(EventBase)) || property.DeclaringType == typeof(EventBase))
			{
				property.ShouldSerialize = instance =>
				{
					if (property.PropertyName == nameof(EventBase.EventNumber) ||
						property.PropertyName == nameof(EventBase.EventId))
						return false;
					return true;
				};
			}

			if (property.PropertyType.GetInterfaces().Contains(typeof(IAggregateId)))
				property.Converter = new AggregateIdConverter();

			return property;
		}
	}
}
