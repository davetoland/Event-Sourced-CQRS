using System;
using System.Threading.Tasks;
using HelloCQRS.Aggregates;

namespace HelloCQRS.Repositories
{
    public interface IRepository
    {
        Task<T> GetByIdAsync<T>(Guid id) where T : Aggregate;
        Task SaveAsync<T>(T aggregate) where T : Aggregate;
    }
}