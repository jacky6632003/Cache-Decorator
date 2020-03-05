using System;
using System.Linq;

namespace CacheDecorator.Common.Settings
{
    /// <summary>
    /// Class CacheDecoratorSettingsProvider.
    /// Implements the <see cref="Sample.Common.Settings.ICacheDecoratorSettingsProvider" />
    /// </summary>
    /// <seealso cref="Sample.Common.Settings.ICacheDecoratorSettingsProvider" />
    public class CacheDecoratorSettingsProvider : ICacheDecoratorSettingsProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheDecoratorSettingsProvider"/> class.
        /// </summary>
        /// <param name="cacheDecoratorSettings">The cache decorator settings.</param>
        public CacheDecoratorSettingsProvider(CacheDecoratorSettings cacheDecoratorSettings)
        {
            this.CacheDecoratorSettings = cacheDecoratorSettings;
        }

        private CacheDecoratorSettings CacheDecoratorSettings { get; set; }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>CacheDecoratorSettings.</returns>
        public CacheDecoratorSettings GetSettings()
        {
            if (this.CacheDecoratorSettings.EqualNull())
            {
                return CacheDecoratorSettings.Null;
            }

            if (this.CacheDecoratorSettings.CacheProviders.EqualNull()
                &&
                this.CacheDecoratorSettings.CacheDecorators.EqualNull())
            {
                return CacheDecoratorSettings.Null;
            }

            if (this.CacheDecoratorSettings.CacheProviders.Any().Equals(false)
                &&
                this.CacheDecoratorSettings.CacheDecorators.Any().Equals(false))
            {
                return CacheDecoratorSettings.Null;
            }

            return this.CacheDecoratorSettings;
        }
    }
}