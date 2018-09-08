using System;

namespace CQRSE.QueryModel
{
    public interface IProjectionCache
    {
        void CreateOrUpdate(ICacheItem item);
        ICacheItem Retrieve(Type type, Guid id);
    }
}
