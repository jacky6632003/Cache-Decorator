using System;

namespace CacheDecorator.Common.Settings
{
    /// <summary>
    /// interface ISystemSettingsProvider
    /// </summary>
    public interface ISystemSettingsProvider
    {
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        SystemSettings GetSettings();
    }
}