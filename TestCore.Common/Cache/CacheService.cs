using TestCore.Common;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;

namespace TestCore.Common.Cache
{
    public  class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        private TimeSpan _defaultTimeSpan = new TimeSpan(0, 0, 45);//45s过期

        private ConcurrentDictionary<string, object> _lockObjsDic;

        public CacheService(IDistributedCache distributedCache)
        {
            _cache = distributedCache;
            this._lockObjsDic = new ConcurrentDictionary<string, object>();
        }



        public void Put<T>(string key, T value)
        {
            Put<T>(CommonConsts.Cache_Catalog_Default, key, value);
        }

        public void Put<T>(string catalog, string key, T value)
        {
            Put(catalog, key, value, _defaultTimeSpan);
        }


        public void Put<T>(string key, T value, TimeSpan timeSpan )
        {
            Put<T>(CommonConsts.Cache_Catalog_Default, key, value,timeSpan);
        }


        private  void Put<T>(string catalog, string key, T value, TimeSpan timeSpan)
        {
            string cacheKey = GenCacheKey(catalog, key);
            string str = JsonConvert.SerializeObject(value);

            _cache.SetString(cacheKey, str, new DistributedCacheEntryOptions().SetSlidingExpiration(timeSpan));

        }

        public T Get<T>(string key)
        {
            return Get<T>(CommonConsts.Cache_Catalog_Default, key);
        }

        public T Get<T>(string catalog, string key)
        {
            string cacheKey = GenCacheKey(catalog, key);
           // _cache.Remove(cacheKey);
            string str = _cache.GetString(cacheKey);
            if (string.IsNullOrEmpty(str))
                return default(T);
            return JsonConvert.DeserializeObject<T>(str);
        }


        public T GetOrAdd<T>(string key, Func<T> func)
        {
            return GetOrAdd(CommonConsts.Cache_Catalog_Default, key, func, _defaultTimeSpan);
        }


        public T GetOrAdd<T>(string catalog, string key, Func<T> func)
        {
            return GetOrAdd(catalog, key, func, _defaultTimeSpan);
        }


        public T GetOrAdd<T>(string catalog, string key, Func<T> func, TimeSpan timeSpan)
        {
            string cacheKey = GenCacheKey(catalog, key);
          //  _cache.Remove(cacheKey);

            T result = Get<T>(catalog, key);

            if (result == null)
            {
                object lockObj = _lockObjsDic.GetOrAdd(cacheKey, n => new object());
                lock (lockObj)
                {
                    result = Get<T>(catalog, key);

                    if (result == null)
                    {
                        result = func();
                        Put(catalog, key, result, timeSpan);
                    }
                }
            }

            if (result == null)
                return default(T);

            return result;
        }

        public T GetOrAdd<T>(string key, Func<T> func, TimeSpan timeSpan)
        {
            return GetOrAdd<T>(CommonConsts.Cache_Catalog_Default, key, func, timeSpan);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="key"></param>
        public void Remove(string catalog, string key)
        {
            string cacheKey = GenCacheKey(catalog, key);

            _cache.Remove(cacheKey);
        }

        public void Remove( string key)
        {
            Remove(CommonConsts.Cache_Catalog_Default, key);
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void Refresh(string catalog, string key)
        {
            string cacheKey = GenCacheKey(catalog, key);

            _cache.Refresh(cacheKey);
        }

        public void Refresh(string key)
        {
            Refresh(CommonConsts.Cache_Catalog_Default, key);
        }

        /// <summary>
        /// 生成缓存键
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GenCacheKey(string catalog, string key)
        {
            return $"{catalog}-{key}";
        }



    }
}
