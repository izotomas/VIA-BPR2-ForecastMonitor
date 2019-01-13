namespace ForecastMonitor.Test.Integration.TestUtils.TestServices
{
    public interface IMemoryDataContextResetService
    {
        /// <summary>
        /// Clears all data in the cache
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Throws when called in different environment than specified in AllowedEnvironmentsForCacheReset
        /// </exception>
        void ResetCache();
    }
}
