using System;
using System.Threading.Tasks;
using HelloCQRS.Aggregates;
using HelloCQRS.Messages;
using HelloCQRS.Repositories;

namespace HelloCQRS.Services
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