using CacheDecorator.Common.Interface;
using System;
using System.Collections.Generic;

namespace CacheDecorator.Common.Settings
{
    /// <summary>
    /// Class CacheDecoratorSettings.
    /// </summary>
    public class CacheDecoratorSettings : INullObject
    {
        /// <summary>
        /// CacheProvider: NullCacheProvider, MemoryCacheProvider, RedisCacheProvider.
        /// </summary>
        public string[] CacheProviders { get; set; }

        /// <summary>
        /// 快取裝飾者的定義與實做.
        /// </summary>
        public CacheDecorator[] CacheDecorators { get; set; }

        //-----------------------------------------------------------------------------------------
        // Null

        public virtual bool IsNull()
        {
            return false;
        }

        private static CacheDecoratorSettings _null;

        public static CacheDecoratorSettings Null
        {
            get
            {
                if (_null.EqualNull())
                {
                    _null = new NullCacheDecoratorSettings();
                }

                return _null;
            }
        }

        private class NullCacheDecoratorSettings : CacheDecoratorSettings
        {
            public NullCacheDecoratorSettings()
            {
                this.CacheProviders = new[] { "NullCacheProvider" };
                this.CacheDecorators = new List<CacheDecorator>().ToArray();
            }

            public override bool IsNull()
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Class CacheDecorator.
    /// </summary>
    public class CacheDecorator
    {
        /// <summary>
        /// 定義 (interface name)
        /// </summary>
        public string Declaration { get; set; }

        /// <summary>
        /// 實做類別名稱
        /// </summary>
        public string[] Implements { get; set; }
    }
}