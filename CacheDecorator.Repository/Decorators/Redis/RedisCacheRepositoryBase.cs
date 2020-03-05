using CacheDecorator.Common;
using CacheDecorator.Common.Caching;
using CacheDecorator.Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CacheDecorator.Repository.Decorators.Redis
{
    /// <summary>
    /// Class RedisCacheRepositoryBase.
    /// Implements the <see cref="Sample.Repository.Decorators.CachedRepositoryBase" />
    /// </summary>
    /// <seealso cref="Sample.Repository.Decorators.CachedRepositoryBase" />
    public class RedisCacheRepositoryBase : CachedRepositoryBase
    {
        protected const string CachekeyPrefix = "::";

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheRepositoryBase"/> class.
        /// </summary>
        /// <param name="cacheDecoratorSettingsProvider">The cacheDecoratorSettingsProvider.</param>
        /// <param name="cacheProviderResolver">The cacheProviderResolver.</param>
        public RedisCacheRepositoryBase(ICacheDecoratorSettingsProvider cacheDecoratorSettingsProvider,
                                        ICacheProviderResolver cacheProviderResolver)
            : base(cacheDecoratorSettingsProvider, cacheProviderResolver)
        {
        }

        /// <summary>
        /// 取得(建立)快取資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="cacheItemExpiration">The cache item expiration.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override T GetOrAddCacheItem<T>(string cachekey, TimeSpan cacheItemExpiration, Func<T> source)
        {
            if (this.CacheProvider.Exists(cachekey))
            {
                var cachedValue = this.CacheProvider.Get<T>(cachekey);
                if (cachedValue.NotEqualNull())
                {
                    return cachedValue;
                }
            }

            var returnResult = source();

            if (returnResult.EqualNull())
            {
                return returnResult;
            }

            this.CacheProvider.Save(key: cachekey, value: returnResult, cacheTime: cacheItemExpiration);

            return returnResult;
        }

        /// <summary>
        /// 取得(建立)快取資料 asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="cacheItemExpiration">The cache item expiration.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected override async Task<T> GetOrAddCacheItemAsync<T>(string cachekey, TimeSpan cacheItemExpiration, Func<Task<T>> source)
        {
            if (this.CacheProvider.Exists(cachekey))
            {
                var cachedValue = this.CacheProvider.Get<T>(cachekey);
                if (cachedValue.NotEqualNull())
                {
                    return cachedValue;
                }
            }

            var returnResult = await source();

            if (returnResult.EqualNull())
            {
                return returnResult;
            }

            this.CacheProvider.Save(key: cachekey, value: returnResult, cacheTime: cacheItemExpiration);

            return returnResult;
        }

        /// <summary>
        /// 移除指定 cachekey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <returns></returns>
        protected override bool RemoveCacheItem(string cachekey)
        {
            if (this.CacheProvider.Exists(cachekey).Equals(false))
            {
                return false;
            }

            var result = this.CacheProvider.Remove(cachekey);

            return result;
        }

        /// <summary>
        /// 移除指定 cachekey 的快取資料並移除符合 primaryKey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected override void RemoveCacheItem(string cachekey, object primaryKey)
        {
            var keys = new List<string> { cachekey };

            var collection = RedisCacheProvider.Cachekeys
                                               .Where(x => x.Contains(primaryKey.ToString(), StringComparison.OrdinalIgnoreCase))
                                               .ToList();

            keys.AddRange(collection);

            foreach (var key in keys)
            {
                if (this.CacheProvider.Exists(key).Equals(false))
                {
                    continue;
                }

                this.CacheProvider.Remove(key);
            }
        }

        /// <summary>
        /// 移除 CacheKey 開頭符合指定 keyPrefix 的快取資料
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        protected override void RemoveCacheItemByKeyPrefix(string keyPrefix)
        {
            var keys = new List<string>();

            var collection = RedisCacheProvider.Cachekeys
                                               .Where(x => x.StartsWith(keyPrefix, StringComparison.OrdinalIgnoreCase))
                                               .ToList();

            keys.AddRange(collection);

            foreach (var key in keys)
            {
                if (this.CacheProvider.Exists(key).Equals(false))
                {
                    continue;
                }

                this.CacheProvider.Remove(key);
            }
        }
    }
}