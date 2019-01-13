using System;
using ForecastMonitor.Service.DataAccessLogic.DataContext;
using Microsoft.Extensions.Caching.Memory;

namespace ForecastMonitor.Test.Integration.TestUtils.TestServices
{
    public class MemoryDataContextResetService : IMemoryDataContextResetService
    {
        private readonly IMemoryCache _cache;

        public MemoryDataContextResetService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void ResetCache()
        {
            foreach (var key in Enum.GetValues(typeof(MemoryDataContext.Key)))
            {
                this._cache.Remove(key);
            }
        }
    }
}