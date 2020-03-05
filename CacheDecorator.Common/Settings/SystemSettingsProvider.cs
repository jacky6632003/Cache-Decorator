using CacheDecorator.Common;
using CacheDecorator.Common.Settings;
using System;

namespace Sample.Common.Settings
{
    /// <summary>
    /// class SystemSettingsProvider
    /// </summary>
    /// <seealso cref="ISystemSettingsProvider" />
    public class SystemSettingsProvider : ISystemSettingsProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemSettingsProvider" /> class.
        /// </summary>
        /// <param name="systemSettings">The system settings.</param>
        public SystemSettingsProvider(SystemSettings systemSettings)
        {
            this.SystemSettings = systemSettings;
        }

        private SystemSettings SystemSettings { get; set; }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public SystemSettings GetSettings()
        {
            if (this.SystemSettings.EqualNull())
            {
                return SystemSettings.Null;
            }

            if (this.SystemSettings.ServiceName.IsNullOrWhiteSpace()
                && this.SystemSettings.ServiceVersion.IsNullOrWhiteSpace()
                && this.SystemSettings.ServiceDescription.IsNullOrWhiteSpace())
            {
                return SystemSettings.Null;
            }

            return this.SystemSettings;
        }
    }
}