using Com.ByteAnalysis.IFacilita.DocuSign.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace Com.ByteAnalysis.IFacilita.DocuSign.Caching
{
    public class MemoryCache<TEntity> : ICache<TEntity> where TEntity : class
    {
        private IMemoryCache _memoryCache;

        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task AddAsync(string key, TEntity obj, DateTime expirationDate)
        {
            return Task.FromResult(_memoryCache.Set(key, obj, expirationDate));
        }

        public Task<TEntity> GetAsync(string key)
        {
            return Task.FromResult(_memoryCache.Get(key) as TEntity);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
