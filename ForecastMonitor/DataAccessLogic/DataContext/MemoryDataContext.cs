using System;
using System.Collections.Generic;
using System.Linq;
using ForecastMonitor.Service.DataAccessLogic.DataAccessObjects;
using Microsoft.Extensions.Caching.Memory;

namespace ForecastMonitor.Service.DataAccessLogic.DataContext
{
    public class MemoryDataContext : IDataContext
    {
        private const double TwelveHoursInSec = 12 * 60 * 60;
        private readonly double _cacheEntryExpirationInSec;

        private readonly IMemoryCache _cache;
        public enum Key
        {
            Installations,
            Clients,
            Units,
            Predictions,
            TimeSeries,
            ModelInfos
        }

        public MemoryDataContext(IMemoryCache cache, double cacheEntryExpirationInSec = TwelveHoursInSec)
        {
            this._cache = cache;
            this._cacheEntryExpirationInSec = cacheEntryExpirationInSec;
        }

        public List<DaoInstallation> Installations
        {
            get => Get<DaoInstallation>(Key.Installations);
            set => Set<DaoInstallation>(Key.Installations, value);
        }

        public List<DaoClient> Clients
        {
            get => Get<DaoClient>(Key.Clients);
            set => Set<DaoClient>(Key.Clients, value);
        }

        public List<DaoUnit> Units
        {
            get => Get<DaoUnit>(Key.Units);
            set => Set<DaoUnit>(Key.Units, value);
        }

        public List<DaoPrediction> Predictions
        {
            get => Get<DaoPrediction>(Key.Predictions);
            set => Set<DaoPrediction>(Key.Predictions, value);
        }

        public List<DaoTimeSerie> TimeSeries
        {
            get => Get<DaoTimeSerie>(Key.TimeSeries);
            set => Set<DaoTimeSerie>(Key.TimeSeries, value);
        }

        public List<DaoModel> Models
        {
            get => Get<DaoModel>(Key.ModelInfos);
            set => Set<DaoModel>(Key.ModelInfos, value);
        }

        #region Getters & Setters

        private List<T> Get<T>(Key key) where T : IDao
        {
            var items = this._cache.GetOrCreate(key, factory => new List<T>());
            items.RemoveAll(HasExpired);
            return items;
        }

        private void Set<T>(Key key, List<T> values) where T : IDao
        {
            var current = Get<T>(key);
            foreach (var item in values)
            {
                Upsert(key, current, item);
            }
        }

        private void Upsert<T>(Key key, List<T> list, T item) where T : IDao
        {
            var current = list.FirstOrDefault(element => element.PrimaryKeyEquals(item));
            if (current != null)
            {
                list.Remove(current);
            }
            list.Add(item);
            this._cache.Set(key, list, TimeSpan.FromSeconds(_cacheEntryExpirationInSec));
        }

        private bool HasExpired<T>(T item) where T : IDao
        {
            return (DateTime.Now - item.TimeStamp).TotalSeconds > _cacheEntryExpirationInSec;
        }

        #endregion
    }
}
