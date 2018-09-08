using System;
using System.Collections.Generic;
using CQRSE.Messages;
using CQRSE.ServiceBus;

namespace CQRSE.Aggregates
{
    public abstract class Aggregate
    {
        protected IServiceBus ServiceBus;
        protected List<IEvent> UncommittedEvents;

        public Guid Id { get; protected set; }
        public int Version  { get; protected set; }
        public int EventCount => UncommittedEvents.Count;

        protected Aggregate(IServiceBus bus, IEnumerable<IEvent> events)
        {
            ServiceBus = bus;
            UncommittedEvents = new List<IEvent>();

            foreach (var e in events)
                HandleEvent(e);
        }

        protected void HandleEvent(IEvent e)
        {
            Version++;
            Invoker.InvokeEvent(this, e);
        }

        protected void ProcessNewEvent(IEvent e)
        {
            HandleEvent(e);
            UncommittedEvents.Add(e);
            ServiceBus.Publish(e);
        }

        public IEnumerable<IEvent> GetUncommittedEvents()
        {
            return UncommittedEvents;
        }

        public void ClearUncommittedEvents()
        {
            UncommittedEvents.Clear();
        }
    }
}
