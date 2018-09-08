using System;
using System.Threading.Tasks;
using Autofac;
using CQRSE.Messages;
using MassTransit;

namespace CQRSE.ServiceBus.MassTransitBus
{
    public class MassTransitServiceBus : IServiceBus
    {
        private readonly IBusControl _massTransitBus;

        public MassTransitServiceBus(IComponentContext context)
        {
            var config = context.Resolve<IServiceBusConfiguration>();
            if (config is MassTransitServiceBusConfiguration cfg)
            {
                _massTransitBus = Bus.Factory.CreateUsingRabbitMq(x =>
                {
                    x.Host(config.Host, h =>
                    {
                        h.Username(cfg.Username);
                        h.Password(cfg.Password);
                    });
                    
                    x.ReceiveEndpoint(cfg.QueueName, e => e.LoadFrom(context));
                });
            }
            else
                throw new ArgumentException("IServiceBusFactory must be of type MassTransitServiceBusConfiguration");
        }

        public void Start()
        {
            _massTransitBus.Start();
        }

        public void Stop()
        {
            _massTransitBus.Stop();
        }

        public async Task Publish(IEvent eventMessage)
        {
            await _massTransitBus.Publish(eventMessage, eventMessage.GetType());
        }

        public async Task Send(ICommand commandMessage)
        {
            await _massTransitBus.Publish(commandMessage, commandMessage.GetType());
        }
    }
}
