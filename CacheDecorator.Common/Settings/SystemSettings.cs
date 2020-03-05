using CacheDecorator.Common.Interface;
using System;

namespace CacheDecorator.Common.Settings
{
    /// <summary>
    /// class SystemSettings
    /// </summary>
    public class SystemSettings : INullObject
    {
        /// <summary>
        /// 服務名稱
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 版本號
        /// </summary>
        public string ServiceVersion { get; set; }

        /// <summary>
        /// 服務描述
        /// </summary>
        public string ServiceDescription { get; set; }

        //-----------------------------------------------------------------------------------------

        public virtual bool IsNull()
        {
            return false;
        }

        private static SystemSettings _null;

        public static SystemSettings Null
        {
            get
            {
                if (_null.EqualNull())
                {
                    _null = new NullSystemSettings();
                }

                return _null;
            }
        }

        private class NullSystemSettings : SystemSettings
        {
            public NullSystemSettings()
            {
                this.ServiceName = "Sample.WebApplication";
                this.ServiceVersion = "";
                this.ServiceDescription = "Sample.WebApplication";
            }

            public override bool IsNull()
            {
                return true;
            }
        }
    }
}