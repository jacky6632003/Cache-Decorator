using System;
using System.Collections.Generic;

namespace CacheDecorator.Common.Caching
{
    /// <summary>
    /// abstract CacheProvider.
    /// </summary>
    /// <seealso cref="ICacheProvider" />
    public abstract class CacheProvider : ICacheProvider
    {
        protected readonly TimeSpan DefaultDuration;

        protected CacheProvider(TimeSpan duration)
        {
            this.DefaultDuration = duration;
        }

        #region abstract ICacheProvider members

        /// <summary>
        /// Validates if an object with this key exists in the cache
        /// </summary>
        /// <param name="key">The key of the object to check</param>
        /// <returns>
        /// True if it exists, false if it doesn't
        /// </returns>
        public abstract bool Exists(string key);

        /// <summary>
        /// Insert an item in the cache with the expiration that it will expire if not used past its window
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The object to insert</param>
        /// <param name="slidingExpiration">The expiration window</param>
        /// <returns></returns>
        public abstract bool Save(string key, object value, TimeSpan slidingExpiration);

        /// <summary>
        /// Insert an item in the cache with the expiration that will expire at a specific point in time
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The object to insert</param>
        /// <param name="absoluteExpiration">The DateTime in which this object will expire</param>
        /// <returns></returns>
        public abstract bool Save(string key, object value, DateTime absoluteExpiration);

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheTime">The cache time (Minutes).</param>
        /// <returns></returns>
        public abstract bool Save(string key, object value, int cacheTime);

        /// <summary>
        /// Insert an item in the cache with the CacheItemPolicy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        public abstract bool Save<T>(string key, T value, TimeSpan cacheTime);

        /// <summary>
        /// Insert an Collection in the cache with the CacheItemPolicy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyPrefix">The key prefix.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        public abstract bool SaveCollection<T>(string keyPrefix, List<T> collection, TimeSpan cacheTime);

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        /// IEnumerable&lt;T&gt;.
        /// </returns>
        public abstract IEnumerable<T> GetCollection<T>(string key);

        /// <summary>
        /// Gets the item associated with this key if present.
        /// </summary>
        /// <param name="key">An object identifying the requested entry.</param>
        /// <param name="value">The located value or null.</param>
        /// <returns>True if the key was found.</returns>
        public abstract bool TryGetValue(string key, out object value);

        /// <summary>
        /// Retrieves a cached object from the cache
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>
        /// The object from the database, or an exception if the object doesn't exist
        /// </returns>
        public abstract object Get(string key);

        /// <summary>
        /// Retrieves a cached object from the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key used to identify this object.</param>
        /// <returns>
        /// T.
        /// </returns>
        public abstract T Get<T>(string key);

        /// <summary>
        /// Gets a set of cached objects
        /// </summary>
        /// <param name="keys">All of the keys of the objects we wish to retrieve</param>
        /// <returns>
        /// A key / value dictionary containing all of the keys and objects we wanted to retrieve
        /// </returns>
        public abstract IDictionary<string, object> Get(string[] keys);

        /// <summary>
        /// Removes an object from the cache with the specified key
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>
        /// True if the object was removed, false if it didn't exist or was unable to be removed
        /// </returns>
        public abstract bool Remove(string key);

        /// <summary>
        /// Flush this instance.
        /// </summary>
        public abstract void Flush();

        #endregion abstract ICacheProvider members

        #region Implementation of ICacheService

        /// <summary>
        /// Insert an item into the cache with no specifics as to how it will be used
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The value to be saved to the cache</param>
        /// <returns></returns>
        public bool Save(string key, object value)
        {
            return this.Save
            (
                key: key,
                value: value,
                slidingExpiration: this.GetDefaultPolicy()
            );
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object this[string key]
        {
            get => this.Get(key);
            set => this.Save(key, value, this.GetDefaultPolicy());
        }

        #endregion Implementation of ICacheService

        #region Expiration Policy Helpers

        protected TimeSpan GetDefaultPolicy()
        {
            return CacheUtility.GetCacheItemExpiration(this.DefaultDuration);
        }

        #endregion Expiration Policy Helpers
    }
}