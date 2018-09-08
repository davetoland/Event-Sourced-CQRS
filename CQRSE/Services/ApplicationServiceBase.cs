using System;
using System.Threading.Tasks;
using CQRSE.Aggregates;
using CQRSE.Messages;
using CQRSE.Repositories;

namespace CQRSE.Services
{
    public class ApplicationServiceBase
    {
        private readonly IRepository _repository;

        public ApplicationServiceBase(IRepository repository)
        {
            _repository = repository;
        }

        protected void DispatchCommand<T>(T target, ICommand cmd)
        {
            Invoker.InvokeCommand(typeof(T), cmd);
        }

        protected async Task ActAsync<T>(Guid id, Action<T> action) where T: Aggregate
        {
            var aggregate = await _repository.GetByIdAsync<T>(id);
            action(aggregate);
            await _repository.SaveAsync(aggregate);
        }
    }
}