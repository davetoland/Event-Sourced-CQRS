using System.Collections.Generic;
using CQRSE.Messages;
using CQRSE.ServiceBus;

namespace CQRSE.Aggregates
{
    public class AggregateFactory : IAggregateFactory
    {
        public T Hydrate<T>(IServiceBus bus, IEnumerable<IEvent> events) where T : Aggregate
        {
            var ctor = typeof(T).GetConstructor(new[] { typeof(IServiceBus), typeof(IEnumerable<IEvent>) });
            var t = (T)ctor?.Invoke(new object[] { bus, events });
            return t;
        }
    }
}