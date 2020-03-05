using CacheDecorator.Common;
using CacheDecorator.Common.Caching;
using CacheDecorator.Common.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CacheDecorator.Repository.Decorators
{
    /// <summary>
    /// class CachedRepositoryBase
    /// </summary>
    public abstract class CachedRepositoryBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRepositoryBase"/> class.
        /// </summary>
        /// <param name="cacheDecoratorSettingsProvider">The cacheDecoratorSettingsProvider.</param>
        /// <param name="cacheProviderResolver">The cacheProviderResolver.</param>
        protected CachedRepositoryBase(ICacheDecoratorSettingsProvider cacheDecoratorSettingsProvider,
                                       ICacheProviderResolver cacheProviderResolver)
        {
            this.CacheDecoratorSettingsProvider = cacheDecoratorSettingsProvider;
            this.CacheProviderResolver = cacheProviderResolver;
        }

        private ICacheDecoratorSettingsProvider CacheDecoratorSettingsProvider { get; set; }

        private ICacheProviderResolver CacheProviderResolver { get; set; }

        protected ICacheProvider CacheProvider { get; private set; }

        /// <summary>
        /// Sets the CacheProvider.
        /// </summary>
        /// <param name="cacheType">Type of the cache.</param>
        /// <param name="declaration">The declaration.</param>
        /// <param name="implement">The implement.</param>
        protected void SetCacheProvider(CacheTypeEnum cacheType, string declaration, string implement)
        {
            var settings = this.CacheDecoratorSettingsProvider.GetSettings();
            var cacheProviders = settings.CacheProviders;
            var cacheDecorators = settings.CacheDecorators;

            var isDefinedCacheProvider = cacheProviders.Any(x => x.Equals(cacheType.EnumDescription(), StringComparison.OrdinalIgnoreCase));
            if (isDefinedCacheProvider.Equals(false))
            {
                this.CacheProvider = this.CacheProviderResolver.GetCacheProvider(CacheTypeEnum.None);
                return;
            }

            var decorator = cacheDecorators.FirstOrDefault(x => x.Declaration.Equals(declaration, StringComparison.OrdinalIgnoreCase));
            if (decorator.EqualNull())
            {
                this.CacheProvider = this.CacheProviderResolver.GetCacheProvider(CacheTypeEnum.None);
                return;
            }

            if (decorator.Implements.EqualNull() || decorator.Implements.Any().Equals(false))
            {
                this.CacheProvider = this.CacheProviderResolver.GetCacheProvider(CacheTypeEnum.None);
                return;
            }

            if (decorator.Implements.Any(x => x.Equals(implement, StringComparison.OrdinalIgnoreCase)).Equals(false))
            {
                this.CacheProvider = this.CacheProviderResolver.GetCacheProvider(CacheTypeEnum.None);
                return;
            }

            this.CacheProvider = this.CacheProviderResolver.GetCacheProvider(cacheType);
        }

        /// <summary>
        /// 取得(建立)快取資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="cacheItemExpiration">The cache item expiration.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected abstract T GetOrAddCacheItem<T>(string cachekey, TimeSpan cacheItemExpiration, Func<T> source);

        /// <summary>
        /// 取得(建立)快取資料 asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="cacheItemExpiration">The cache item expiration.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        protected abstract Task<T> GetOrAddCacheItemAsync<T>(string cachekey, TimeSpan cacheItemExpiration, Func<Task<T>> source);

        /// <summary>
        /// 移除指定 cachekey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <returns></returns>
        protected abstract bool RemoveCacheItem(string cachekey);

        /// <summary>
        /// 移除指定 cachekey 的快取資料並移除符合 primaryKey 的快取資料
        /// </summary>
        /// <param name="cachekey">The cachekey.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected abstract void RemoveCacheItem(string cachekey, object primaryKey);

        /// <summary>
        /// 移除 CacheKey 開頭符合指定 keyPrefix 的快取資料
        /// </summary>
        /// <param name="keyPrefix">The key prefix.</param>
        protected abstract void RemoveCacheItemByKeyPrefix(string keyPrefix);
    }
}