using System;

namespace CQRSE.QueryModel
{
    public class Projection
    {
        private readonly IProjectionCache _cache;

        public Projection(IProjectionCache cache) => _cache = cache;

        protected void Add(ICacheItem obj)
        {
            _cache.CreateOrUpdate(obj);
        }

        protected void Update(Type projectionType, Guid id, Action<ICacheItem> updateAction)
        {
            var projection = _cache.Retrieve(projectionType, id);
            if (projection != null)
                updateAction(projection);
            else
                throw new ArgumentNullException($"Could not find a cache item [projection type {projectionType.Name}] with id: {id}");
        }
    }
}
