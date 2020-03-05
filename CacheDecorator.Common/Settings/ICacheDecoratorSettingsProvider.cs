using System;

namespace CacheDecorator.Common.Settings
{
    /// <summary>
    /// Interface ICacheDecoratorSettingsProvider
    /// </summary>
    public interface ICacheDecoratorSettingsProvider
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        CacheDecoratorSettings GetSettings();
    }
}