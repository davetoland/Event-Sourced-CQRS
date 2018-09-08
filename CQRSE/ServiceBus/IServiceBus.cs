using System.Threading.Tasks;
using HelloCQRS.Messages;

namespace HelloCQRS.ServiceBus
{
    public interface IServiceBus
    {
        void Start();
        void Stop();
        Task Send(ICommand cmd);
        Task Publish(IEvent evt);
    }
}
