using System;

namespace CacheDecorator.Common.Caching
{
    public enum CacheTypeEnum
    {
        /// <summary>
        /// None - NullCacheProvider
        /// </summary>
        [EnumDescription("NullCacheProvider")]
        None = 0,

        /// <summary>
        /// Memory - MemoryCacheProvider
        /// </summary>
        [EnumDescription("MemoryCacheProvider")]
        Memory = 1,

        /// <summary>
        /// Redis - RedisCacheProvider
        /// </summary>
        [EnumDescription("RedisCacheProvider")]
        Redis = 2
    }
}