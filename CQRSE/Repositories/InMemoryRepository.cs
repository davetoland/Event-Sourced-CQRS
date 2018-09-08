using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSE.Aggregates;
using CQRSE.Messages;
using CQRSE.ServiceBus;

namespace CQRSE.Repositories
{
    public class InMemoryRepository : IRepository
    {
        private readonly IServiceBus _bus;
        private readonly IAggregateFactory _aggregateFactory;
        private readonly Dictionary<Type, List<EventData>> _events;

        public InMemoryRepository(IServiceBus bus, IAggregateFactory aggregateFactory)
        {
            _bus = bus;
            _aggregateFactory = aggregateFactory;
            _events = new Dictionary<Type, List<EventData>>();
        }

        public async Task<T> GetByIdAsync<T>(Guid id) where T : Aggregate
        {
            var result = GetById<T>(id);
            return await Task.FromResult(result);
        }

        public async Task SaveAsync<T>(T aggregate) where T : Aggregate
        {
            Save(aggregate);
            await Task.FromResult(0);
        }

        private T GetById<T>(Guid id) where T : Aggregate
        {
            var raw = _events.ContainsKey(typeof(T)) ?
                _events[typeof(T)].Where(e => e.AggregateId == id) : 
                new List<EventData>().AsEnumerable();
            var events = raw.Select(x => x.DeserializeEvent());
            var aggregate = _aggregateFactory.Hydrate<T>(_bus, events);
            return aggregate;
        }

        private void Save<T>(T aggregate) where T : Aggregate
        {
            if (aggregate.EventCount == 0)
                return;

            var aggregateType = aggregate.GetType().Name;
            var originalVersion = aggregate.Version - aggregate.EventCount + 1;
            var eventsToSave = aggregate.GetUncommittedEvents()
                .Select(e => e.ToEventData(aggregateType, aggregate.Id, originalVersion++))
                .ToList();

            if (_events.ContainsKey(typeof(T)))
            {
                var foundVersion = _events[typeof(T)]
                    .Where(e => e.AggregateId == aggregate.Id)
                    .Max(e => e.Version);

                if (foundVersion >= originalVersion)
                    throw new Exception("Concurrency exception");


                _events[typeof(T)].AddRange(eventsToSave);
            }
            else
                _events.Add(typeof(T), eventsToSave);

            aggregate.ClearUncommittedEvents();
        }
    }
}
