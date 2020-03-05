using System;

namespace CacheDecorator.Common.Caching
{
    /// <summary>
    /// Class CacheUtility.
    /// </summary>
    public class CacheUtility
    {
        /// <summary>
        /// Gets the cache item Expiration (default: 5 min).
        /// </summary>
        /// <param name="timeSpan">The minutes.</param>
        /// <returns></returns>
        public static TimeSpan GetCacheItemExpiration(TimeSpan? timeSpan)
        {
            var absoluteExpiration = timeSpan.EqualNull()
                ? TimeSpan.FromMinutes(5)
                : timeSpan.HasValue
                    ? TimeSpan.FromSeconds(timeSpan.Value.TotalSeconds)
                    : TimeSpan.FromMinutes(5);

            return absoluteExpiration;
        }

        /// <summary>
        /// Gets the cache item Expiration - 1 min.
        /// </summary>
        /// <returns>TimeSpan.</returns>
        public static TimeSpan GetCacheItemExpiration1Min()
        {
            return TimeSpan.FromMinutes(1);
        }

        /// <summary>
        /// Gets the cache item Expiration - 5 min.
        /// </summary>
        /// <returns>TimeSpan.</returns>
        public static TimeSpan GetCacheItemExpiration5Min()
        {
            return TimeSpan.FromMinutes(5);
        }

        /// <summary>
        /// Gets the cache item Expiration - 10 min.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetCacheItemExpiration10Min()
        {
            return TimeSpan.FromMinutes(10);
        }

        /// <summary>
        /// Gets the cache Expiration - 30 minutes.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetCacheItemExpiration30Min()
        {
            return TimeSpan.FromMinutes(30);
        }

        /// <summary>
        /// Gets the cache Expiration - one hour.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetCacheItemExpirationOneHour()
        {
            return TimeSpan.FromHours(1);
        }

        /// <summary>
        /// Gets the cache Expiration - 6 hour.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetCacheItemExpiration6Hour()
        {
            return TimeSpan.FromHours(6);
        }

        /// <summary>
        /// Gets the cache Expiration - one day.
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetCacheItemExpirationOneDay()
        {
            return TimeSpan.FromDays(1);
        }
    }
}