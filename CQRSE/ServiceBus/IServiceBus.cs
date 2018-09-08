using System.Threading.Tasks;
using CQRSE.Messages;

namespace CQRSE.ServiceBus
{
    public interface IServiceBus
    {
        void Start();
        void Stop();
        Task Send(ICommand cmd);
        Task Publish(IEvent evt);
    }
}
