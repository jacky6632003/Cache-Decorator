using System;
using System.Collections.Generic;
using System.Text;

namespace CacheDecorator.Common
{
    public static class SystemTime
    {
        public static Func<DateTime> SetCurrentUtcTime;

        public static Func<DateTime> SetCurrentTime;

        public static Func<DateTime> SetToday;

        public static DateTime Now
        {
            get
            {
                return SystemTime.SetCurrentTime();
            }
        }

        public static DateTime Today
        {
            get
            {
                return SystemTime.SetToday();
            }
        }

        public static DateTime UtcNow
        {
            get
            {
                return SystemTime.SetCurrentUtcTime();
            }
        }

        static SystemTime()
        {
            SystemTime.SetCurrentUtcTime = () => DateTime.UtcNow;
            SystemTime.SetCurrentTime = () => DateTime.Now;
            SystemTime.SetToday = () => DateTime.Today;
        }
    }
}