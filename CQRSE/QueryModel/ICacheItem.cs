using System;

namespace HelloCQRS.QueryModel
{
    public interface ICacheItem
    {
        Guid Id { get; }
    }
}
