using System;
using System.Collections.Generic;
using HelloCQRS.Messages;
using HelloCQRS.ServiceBus;

namespace HelloCQRS.Aggregates
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
                ApplyEvent(e);
        }

        protected void ApplyEvent(IEvent e)
        {
            Version++;
            Invoker.InvokeEvent(this, e);
        }

        protected void ProcessNewEvent(IEvent e)
        {
            ApplyEvent(e);
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
