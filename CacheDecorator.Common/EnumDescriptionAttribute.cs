using System;
using System.Collections.Generic;
using System.Text;

namespace CacheDecorator.Common
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
    public sealed class EnumDescriptionAttribute : Attribute
    {
        public string Description
        {
            get;
        }

        public EnumDescriptionAttribute(string description)
        {
            this.Description = description;
        }
    }
}