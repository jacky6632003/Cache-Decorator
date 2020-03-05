using System;
using System.Collections.Generic;
using System.Text;

namespace CacheDecorator.Common
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class StringValueAttribute : Attribute
    {
        internal bool Preferred
        {
            get;
            private set;
        }

        internal string StringValue
        {
            get;
            private set;
        }

        public StringValueAttribute(string value, bool preferred = false)
        {
            this.StringValue = value;
            this.Preferred = preferred;
        }
    }
}