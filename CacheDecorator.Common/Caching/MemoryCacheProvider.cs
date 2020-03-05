using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace CacheDecorator.Common.Caching
{
    /// <summary>
    /// class MemoryCacheProvider.
    /// </summary>
    /// <seealso cref="CacheProvider" />
    public class MemoryCacheProvider : CacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// All keys of cache
        /// </summary>
        /// <remarks>Dictionary value indicating whether a key still exists in cache</remarks>
        private static ConcurrentDictionary<string, bool> AllKeys;

        public static IReadOnlyCollection<string> Cachekeys
        {
            get { return AllKeys.Select(x => x.Key).ToList().AsReadOnly(); }
        }

        /// <summary>
        /// Cancellation token for clear cache
        /// </summary>
        protected CancellationTokenSource CancellationTokenSource;

        static MemoryCacheProvider()
        {
            AllKeys = new ConcurrentDictionary<string, bool>();
        }

        public MemoryCacheProvider(IMemoryCache cache)
            : base(new TimeSpan(0, 20, 0))
        {
            this._memoryCache = cache;
            this.CancellationTokenSource = new CancellationTokenSource();
        }

        //-----------------------------------------------------------------------------------------

        private const string LockCacheKeyPrefix = "##LockCacheKey##";

        private static object GetLockObject(string key)
        {
            var lockObjectKey = $"{LockCacheKeyPrefix}{key}";

            var cache = new MemoryCache(new MemoryCacheOptions());

            lock (cache)
            {
                if (cache.Get(lockObjectKey).NotEqualNull())
                {
                    return cache.Get(lockObjectKey);
                }

                var options = new MemoryCacheEntryOptions().SetSlidingExpiration(new TimeSpan(0, 10, 0));
                cache.Set(lockObjectKey, new object(), options);
            }

            return cache.Get(lockObjectKey);
        }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Create entry options to item of memory cache
        /// </summary>
        /// <param name="cacheTime">Cache time</param>
        protected MemoryCacheEntryOptions GetMemoryCacheEntryOptions(TimeSpan cacheTime)
        {
            var expirationToken = new CancellationChangeToken
            (
                new CancellationTokenSource(cacheTime + new TimeSpan(0, 0, 0, 1)).Token
            );

            var options = new MemoryCacheEntryOptions()
                          .SetAbsoluteExpiration(cacheTime)
                          .AddExpirationToken(expirationToken)
                          .RegisterPostEvictionCallback(this.PostEviction, state: this);

            return options;
        }

        /// <summary>
        /// Add key to dictionary
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>Itself key</returns>
        protected string AddKey(string key)
        {
            AllKeys.TryAdd(key, true);
            return key;
        }

        /// <summary>
        /// Remove key from dictionary
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <returns>Itself key</returns>
        protected string RemoveKey(string key)
        {
            this.TryRemoveKey(key);
            return key;
        }

        /// <summary>
        /// Try to remove a key from dictionary, or mark a key as not existing in cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        protected void TryRemoveKey(string key)
        {
            // try to remove key from dictionary
            if (AllKeys.TryRemove(key, out _).Equals(false))
            {
                // if not possible to remove key from dictionary, then try to mark key as not existing in cache
                AllKeys.TryUpdate(key, false, true);
            }
        }

        /// <summary>
        /// Remove all keys marked as not existing
        /// </summary>
        private void ClearKeys()
        {
            foreach (var key in AllKeys.Where(p => !p.Value).Select(p => p.Key).ToList())
            {
                this.RemoveKey(key);
            }
        }

        /// <summary>
        /// Post eviction
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="value">Value of cached item</param>
        /// <param name="reason">Eviction reason</param>
        /// <param name="state">State</param>
        private void PostEviction(object key, object value, EvictionReason reason, object state)
        {
            // if cached item just change, then nothing doing
            if (reason == EvictionReason.Replaced)
            {
                return;
            }

            // try to remove all keys marked as not existing
            this.ClearKeys();

            // try to remove this key from dictionary
            this.TryRemoveKey(key.ToString());
        }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Existses the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">key</exception>
        public override bool Exists(string key)
        {
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (GetLockObject(key))
            {
                return this._memoryCache.TryGetValue(key, out _);
            }
        }

        /// <summary>
        /// Insert an item in the cache with the expiration that it will expire if not used past its window
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The object to insert</param>
        /// <param name="slidingExpiration">The expiration window</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// key
        /// or
        /// value
        /// </exception>
        public override bool Save(string key, object value, TimeSpan slidingExpiration)
        {
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value.EqualNull())
            {
                throw new ArgumentNullException(nameof(value));
            }

            lock (GetLockObject(key))
            {
                bool result;

                try
                {
                    this._memoryCache.Set
                    (
                        key: this.AddKey(key),
                        value: value,
                        options: new MemoryCacheEntryOptions().SetSlidingExpiration(slidingExpiration)
                    );

                    result = true;
                }
                catch
                {
                    result = false;
                }

                return result;
            }
        }

        /// <summary>
        /// Insert an item in the cache with the expiration that will expire at a specific point in time
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The object to insert</param>
        /// <param name="absoluteExpiration">The DateTime in which this object will expire</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// key
        /// or
        /// value
        /// </exception>
        public override bool Save(string key, object value, DateTime absoluteExpiration)
        {
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value.EqualNull())
            {
                throw new ArgumentNullException(nameof(value));
            }

            lock (GetLockObject(key))
            {
                bool result;

                try
                {
                    this._memoryCache.Set
                    (
                        key: this.AddKey(key),
                        value: value,
                        options: this.GetMemoryCacheEntryOptions(new TimeSpan(absoluteExpiration.Ticks))
                    );

                    result = true;
                }
                catch
                {
                    result = false;
                }

                return result;
            }
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheTime">The cache time (Minutes).</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// key
        /// or
        /// value
        /// </exception>
        public override bool Save(string key, object value, int cacheTime)
        {
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value.EqualNull())
            {
                throw new ArgumentNullException(nameof(value));
            }

            lock (GetLockObject(key))
            {
                bool result;

                try
                {
                    this._memoryCache.Set
                    (
                        this.AddKey(key),
                        value,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheTime))
                    );

                    result = true;
                }
                catch
                {
                    result = false;
                }

                return result;
            }
        }

        /// <summary>
        /// Insert an item in the cache with the cacheTime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// key
        /// or
        /// value
        /// </exception>
        public override bool Save<T>(string key, T value, TimeSpan cacheTime)
        {
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value.EqualNull())
            {
                throw new ArgumentNullException(nameof(value));
            }

            lock (GetLockObject(key))
            {
                bool result;

                try
                {
                    this._memoryCache.Set
                    (
                        key: this.AddKey(key),
                        value: (object)value,
                        options: this.GetMemoryCacheEntryOptions(cacheTime)
                    );

                    result = true;
                }
                catch
                {
                    result = false;
                }

                return result;
            }
        }

        /// <summary>
        /// Insert an Collection in the cache with the cacheTime.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyPrefix">The key prefix.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// keyPrefix
        /// or
        /// collection
        /// </exception>
        public override bool SaveCollection<T>(string keyPrefix, List<T> collection, TimeSpan cacheTime)
        {
            if (keyPrefix.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(keyPrefix));
            }

            if (collection.EqualNull() || collection.Any().Equals(false))
            {
                throw new ArgumentNullException(nameof(collection));
            }

            lock (GetLockObject(keyPrefix))
            {
                bool result;

                try
                {
                    this._memoryCache.Set
                    (
                        key: this.AddKey(keyPrefix),
                        value: collection,
                        options: new MemoryCacheEntryOptions().SetAbsoluteExpiration(cacheTime)
                    );

                    result = true;
                }
                catch
                {
                    result = false;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the item associated with this key if present.
        /// </summary>
        /// <param name="key">An object identifying the requested entry.</param>
        /// <param name="value">The located value or null.</param>
        /// <returns>True if the key was found.</returns>
        /// <exception cref="ArgumentNullException">key</exception>
        public override bool TryGetValue(string key, out object value)
        {
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (GetLockObject(key))
            {
                var result = this._memoryCache.TryGetValue(key, out var cachedValue);
                value = result ? cachedValue : null;

                return result;
            }
        }

        /// <summary>
        /// Retrieves a cached object from the cache
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>
        /// The object from the database, or an exception if the object doesn't exist
        /// </returns>
        /// <exception cref="ArgumentNullException">key</exception>
        public override object Get(string key)
        {
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (GetLockObject(key))
            {
                return this._memoryCache.TryGetValue(key, out var value) ? value : null;
            }
        }

        /// <summary>
        /// Retrieves a cached object from the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key used to identify this object.</param>
        /// <returns>
        /// T.
        /// </returns>
        /// <exception cref="ArgumentNullException">key</exception>
        public override T Get<T>(string key)
        {
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (GetLockObject(key))
            {
                if (this._memoryCache.TryGetValue(key, out var cachedObject).Equals(false))
                {
                    return default(T);
                }

                if (cachedObject is T variable)
                {
                    return variable;
                }

                try
                {
                    return (T)Convert.ChangeType(cachedObject, typeof(T));
                }
                catch (InvalidCastException)
                {
                    return default(T);
                }
            }
        }

        /// <summary>
        /// Gets a set of cached objects
        /// </summary>
        /// <param name="keys">All of the keys of the objects we wish to retrieve</param>
        /// <returns>
        /// A key / value dictionary containing all of the keys and objects we wanted to retrieve
        /// </returns>
        /// <exception cref="ArgumentNullException">keys</exception>
        public override IDictionary<string, object> Get(string[] keys)
        {
            if (keys.EqualNull() || keys.Any().Equals(false))
            {
                throw new ArgumentNullException(nameof(keys));
            }

            return keys.ToDictionary(key => key, this.Get);
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// IEnumerable&lt;T&gt;.
        /// </returns>
        /// <exception cref="ArgumentNullException">key</exception>
        public override IEnumerable<T> GetCollection<T>(string key)
        {
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (GetLockObject(key))
            {
                if (this._memoryCache.TryGetValue(key, out var cachedObject).Equals(false))
                {
                    return default(IEnumerable<T>);
                }

                try
                {
                    return (IEnumerable<T>)cachedObject;
                }
                catch (InvalidCastException)
                {
                    return default(IEnumerable<T>);
                }
            }
        }

        /// <summary>
        /// Removes an object from the cache with the specified key
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>
        /// True if the object was removed, false if it didn't exist or was unable to be removed
        /// </returns>
        /// <exception cref="ArgumentNullException">key</exception>
        public override bool Remove(string key)
        {
            if (key.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(key));
            }

            lock (GetLockObject(key))
            {
                if (this.Exists(key).Equals(false))
                {
                    return false;
                }

                this._memoryCache.Remove(this.RemoveKey(key));

                return this.Exists(key).Equals(false);
            }
        }

        /// <summary>
        /// Flush this instance.
        /// </summary>
        public override void Flush()
        {
            foreach (var key in AllKeys.Keys)
            {
                this.Remove(key);
            }
        }
    }
}