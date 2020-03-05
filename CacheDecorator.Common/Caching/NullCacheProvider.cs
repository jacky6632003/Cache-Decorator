using System;
using System.Collections.Generic;

namespace CacheDecorator.Common.Caching
{
    /// <summary>
    /// Class NullCacheProvider.
    /// </summary>
    /// <seealso cref="ICacheProvider" />
    public class NullCacheProvider : ICacheProvider
    {
        /// <summary>
        /// Validates if an object with this key exists in the cache
        /// </summary>
        /// <param name="key">The key of the object to check</param>
        /// <returns>True if it exists, false if it doesn't</returns>
        public bool Exists(string key)
        {
            return default(bool);
        }

        /// <summary>
        /// Insert an item into the cache with no specifics as to how it will be used
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The value to be saved to the cache</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Save(string key, object value)
        {
            return default(bool);
        }

        /// <summary>
        /// Insert an item in the cache with the expiration that it will expire if not used past its window
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The object to insert</param>
        /// <param name="slidingExpiration">The expiration window</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Save(string key, object value, TimeSpan slidingExpiration)
        {
            return default(bool);
        }

        /// <summary>
        /// Insert an item in the cache with the expiration that will expire at a specific point in time
        /// </summary>
        /// <param name="key">The key used to map this object</param>
        /// <param name="value">The object to insert</param>
        /// <param name="absoluteExpiration">The DateTime in which this object will expire</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Save(string key, object value, DateTime absoluteExpiration)
        {
            return default(bool);
        }

        /// <summary>
        /// Saves the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheTime">The cache time (Minutes).</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Save(string key, object value, int cacheTime)
        {
            return default(bool);
        }

        /// <summary>
        /// Insert an item in the cache with the CacheItemPolicy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Save<T>(string key, T value, TimeSpan cacheTime)
        {
            return default(bool);
        }

        /// <summary>
        /// Insert an Collection in the cache with the CacheItemPolicy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyPrefix">The key prefix.</param>
        /// <param name="collection">The collection.</param>
        /// <param name="cacheTime">The cache time.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SaveCollection<T>(string keyPrefix, List<T> collection, TimeSpan cacheTime)
        {
            return default(bool);
        }

        /// <summary>
        /// Gets the item associated with this key if present.
        /// </summary>
        /// <param name="key">An object identifying the requested entry.</param>
        /// <param name="value">The located value or null.</param>
        /// <returns>True if the key was found.</returns>
        public bool TryGetValue(string key, out object value)
        {
            value = null;
            return false;
        }

        /// <summary>
        /// Retrieves a cached object from the cache
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>The object from the database, or an exception if the object doesn't exist</returns>
        public object Get(string key)
        {
            return default(object);
        }

        /// <summary>
        /// Retrieves a cached object from the cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key used to identify this object.</param>
        /// <returns>T.</returns>
        public T Get<T>(string key)
        {
            return default(T);
        }

        /// <summary>
        /// Gets a set of cached objects
        /// </summary>
        /// <param name="keys">All of the keys of the objects we wish to retrieve</param>
        /// <returns>A key / value dictionary containing all of the keys and objects we wanted to retrieve</returns>
        public IDictionary<string, object> Get(string[] keys)
        {
            return default(IDictionary<string, object>);
        }

        /// <summary>
        /// Gets the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public IEnumerable<T> GetCollection<T>(string key)
        {
            return default(IEnumerable<T>);
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Object.</returns>
        public object this[string key]
        {
            get => this.Get(key);
            set => this.Save(key, value);
        }

        /// <summary>
        /// Removes an object from the cache with the specified key
        /// </summary>
        /// <param name="key">The key used to identify this object</param>
        /// <returns>True if the object was removed, false if it didn't exist or was unable to be removed</returns>
        public bool Remove(string key)
        {
            return default(bool);
        }

        public void Flush()
        {
            // nothing
        }
    }
}