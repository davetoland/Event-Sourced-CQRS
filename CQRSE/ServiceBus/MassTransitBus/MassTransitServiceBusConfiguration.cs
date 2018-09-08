using System;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace HelloCQRS.ServiceBus.MassTransitBus
{
    public class MassTransitServiceBusConfiguration : IServiceBusConfiguration
    {
        public MassTransitServiceBusConfiguration(IComponentContext context)
        {
            var config = context.Resolve<IConfiguration>();
            var busConfig = config.GetSection("bus");

            if (!Uri.TryCreate(busConfig["host"], UriKind.Absolute, out var host))
                throw new ArgumentException("Host is not a valid Uri");

            Host = host;
            Username = busConfig["user"];
            Password = busConfig["pass"];
            QueueName = busConfig["queue"];
        }

        public Uri Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}
