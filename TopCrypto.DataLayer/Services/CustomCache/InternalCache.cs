using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace TopCrypto.DataLayer.Services.CustomCache
{
    internal class InternalCacheDTO
    {
        public DateTime ExpiredTime { get; set; }
        public object StoredObject { get; set; }
    };

    public enum InternalCacheKeys
    {
        FiatCurrencyDataService = 1,
        CoinInfoDataService = 2,
        WeekPriceAverage = 3,
        News = 4,
        Brokers = 5,
        CoinMarketAndIds = 6,
        Settings = 7
    }

    internal class InternalCacheKey : IEquatable<InternalCacheKey>
    {
        public InternalCacheKey() { }

        public InternalCacheKey(InternalCacheKeys cacheKey, string query)
        {
            CacheKey = cacheKey;
            Query = query;
        }

        public InternalCacheKeys CacheKey { get; set; }
        public string Query { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as InternalCacheKey);
        }

        public bool Equals(InternalCacheKey other)
        {
            return other != null &&
                   CacheKey == other.CacheKey &&
                   Query == other.Query;
        }

        public override int GetHashCode()
        {
            var hashCode = -769493713;
            hashCode = hashCode * -1521134295 + CacheKey.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Query);
            return hashCode;
        }

        public static bool operator ==(InternalCacheKey key1, InternalCacheKey key2)
        {
            return EqualityComparer<InternalCacheKey>.Default.Equals(key1, key2);
        }

        public static bool operator !=(InternalCacheKey key1, InternalCacheKey key2)
        {
            return !(key1 == key2);
        }
    }

    public class InternalCache
    {
        private Dictionary<InternalCacheKeys, Dictionary<InternalCacheKey, InternalCacheDTO>> storage =
            new Dictionary<InternalCacheKeys, Dictionary<InternalCacheKey, InternalCacheDTO>>();

        private ConcurrentDictionary<InternalCacheKeys, object> locks =
            new ConcurrentDictionary<InternalCacheKeys, object>();

        public InternalCache()
        {
            foreach (InternalCacheKeys key in (InternalCacheKeys[])Enum.GetValues(typeof(InternalCacheKeys)))
            {
                locks.TryAdd(key, new object());
                storage.TryAdd(key, new Dictionary<InternalCacheKey, InternalCacheDTO>());
            }
        }

        public void Add(InternalCacheKeys key, object value, int seconds, string query = null)
        {
            if (value == null) return;

            var internalCache = new InternalCacheKey(key, query);
            lock (locks[key])
            {
                var dt = DateTime.Now;
                dt = dt.AddSeconds(seconds);
                var keyStorage = storage[key];

                keyStorage[internalCache] = new InternalCacheDTO() { ExpiredTime = dt, StoredObject = value };
            }
        }

        public void AddUseOldTime(InternalCacheKeys key, object value, int seconds, string query = null)
        {
            if (value == null) return;

            var internalCache = new InternalCacheKey(key, query);
            lock (locks[key])
            {
                var keyStorage = storage[key];

                var flag = keyStorage.TryGetValue(internalCache, out var oldItem);
                if (flag && oldItem != null)
                {
                    keyStorage[internalCache] = new InternalCacheDTO()
                    {
                        ExpiredTime = oldItem.ExpiredTime,
                        StoredObject = value
                    };
                }
                else {
                    var dt = DateTime.Now;
                    dt = dt.AddSeconds(seconds);

                    keyStorage[internalCache] = new InternalCacheDTO()
                    {
                        ExpiredTime = dt,
                        StoredObject = value
                    };
                }
            }
        }

        public object Get(InternalCacheKeys key, string query = null, bool ignoreExpiration = false)
        {
            var internalCache = new InternalCacheKey(key, query);
            lock (locks[key])
            {
                if (!storage.ContainsKey(key)) return null;

                var keyStorage = storage[key];
                if (!keyStorage.TryGetValue(internalCache, out InternalCacheDTO storedObj)) return null;

                if (ignoreExpiration)
                {
                    return storedObj.StoredObject;
                }
                if (storedObj.ExpiredTime > DateTime.Now)
                {
                    return storedObj.StoredObject;
                }

                CleanStorage(key);
                return null;
            }
        }

        public void CleanStorageHard(InternalCacheKeys key)
        {
            lock (locks[key])
            {
                storage[key] = new Dictionary<InternalCacheKey, InternalCacheDTO>();
            }
        }

        private void CleanStorage(InternalCacheKeys key)
        {
            lock (locks[key])
            {
                var keyStorage = storage[key];
                var cacheKyes = new List<InternalCacheKey>();
                foreach (var entryKey in keyStorage.Keys)
                {
                    var storedObj = keyStorage[entryKey];
                    if (storedObj.ExpiredTime <= DateTime.Now)
                    {
                        cacheKyes.Add(entryKey);
                    }
                }

                foreach (var cachKey in cacheKyes)
                {
                    keyStorage.Remove(cachKey, out InternalCacheDTO value);
                }
            }
        }
    }
}
