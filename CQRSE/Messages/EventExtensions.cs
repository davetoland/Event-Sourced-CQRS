using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HelloCQRS.Messages
{
    public static class EventExtensions
    {
        private static readonly JsonSerializerSettings SerializerSettings 
            = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None };

        public static EventData ToEventData(this object evt, string aggregateType, Guid aggregateId, int version)
        {
            var data = JsonConvert.SerializeObject(evt, SerializerSettings);
            var eventHeaders = new Dictionary<string, object>
            {
                { "EventClrType", evt.GetType().AssemblyQualifiedName }
            };
            var metadata = JsonConvert.SerializeObject(eventHeaders, SerializerSettings);
            var eventId = Guid.NewGuid();

            return new EventData
            {
                Id = eventId,
                Created = DateTime.Now,
                AggregateType = aggregateType,
                AggregateId = aggregateId,
                Version = version,
                Event = data,
                Metadata = metadata
            };
        }

        public static IEvent DeserializeEvent(this EventData x)
        {
            var eventClrTypeName = JObject.Parse(x.Metadata).Property("EventClrType").Value;
            return (IEvent)JsonConvert.DeserializeObject(x.Event, Type.GetType((string)eventClrTypeName));
        }
    }
}
