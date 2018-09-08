using System;

namespace HelloCQRS.ServiceBus
{
    public interface IServiceBusConfiguration
    {
        Uri Host { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}