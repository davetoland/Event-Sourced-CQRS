using System.Collections.Generic;
using HelloCQRS.Messages;
using HelloCQRS.ServiceBus;

namespace HelloCQRS.Aggregates
{
    public interface IAggregateFactory
    {
        T Hydrate<T>(IServiceBus bus, IEnumerable<IEvent> events) where T : Aggregate;
    }
}
