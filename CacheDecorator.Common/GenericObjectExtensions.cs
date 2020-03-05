using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CacheDecorator.Common
{
    public static class GenericObjectExtensions
    {
        public static T ConvertTo<T>(this object value)
        {
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
        }

        public static T Deserialize<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static bool EqualNull(this object obj)
        {
            return (object)obj == (object)null;
        }

        public static bool NotEqualNull(this object obj)
        {
            return !obj.EqualNull();
        }

        public static string Serialize<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static int ToInt(this object @object)
        {
            if (@object.EqualNull())
            {
                throw new ArgumentNullException("object", "must input object");
            }
            return @object.ToInt(0);
        }

        public static int ToInt(this object @object, int defaultValue = 0)
        {
            if (@object.EqualNull())
            {
                throw new ArgumentNullException("object", "must input object");
            }
            return @object.ToString().ToInt(defaultValue);
        }
    }
}