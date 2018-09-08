using System.Collections.Generic;
using CQRSE.Messages;
using CQRSE.ServiceBus;

namespace CQRSE.Aggregates
{
    public interface IAggregateFactory
    {
        T Hydrate<T>(IServiceBus bus, IEnumerable<IEvent> events) where T : Aggregate;
    }
}
