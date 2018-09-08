using System;
using System.Threading.Tasks;
using CQRSE.Aggregates;

namespace CQRSE.Repositories
{
    public interface IRepository
    {
        Task<T> GetByIdAsync<T>(Guid id) where T : Aggregate;
        Task SaveAsync<T>(T aggregate) where T : Aggregate;
    }
}