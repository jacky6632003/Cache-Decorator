using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CacheDecorator.Common
{
    public class EnumHelper
    {
        public EnumHelper()
        {
        }

        public static string GetEnumMemberValue(Enum item)
        {
            if (item.EqualNull())
            {
                throw new ArgumentNullException("item");
            }
            EnumMemberAttribute attributeOfType = item.GetAttributeOfType<EnumMemberAttribute>();
            if (!attributeOfType.EqualNull())
            {
                return attributeOfType.Value;
            }
            return item.ToString().ToLower();
        }

        public static bool IsDefined(Type typeOfEnum, object value)
        {
            if (typeOfEnum.EqualNull())
            {
                throw new ArgumentNullException("typeOfEnum");
            }
            if (value.EqualNull())
            {
                throw new ArgumentNullException("value");
            }
            return Enum.IsDefined(typeOfEnum, value);
        }

        public static T ParseEnum<T>(string inString, bool ignoreCase = true, bool throwException = true)
        where T : struct
        {
            return EnumHelper.ParseEnum<T>(inString, default(T), ignoreCase, throwException);
        }

        public static T ParseEnum<T>(string inString, T defaultValue, bool ignoreCase = true, bool throwException = false)
        where T : struct
        {
            T t;
            if (typeof(T).IsEnum.Equals(false) || String.IsNullOrEmpty(inString))
            {
                throw new InvalidOperationException(String.Concat("Invalid Enum Type or Input String 'inString'. ", typeof(T).ToString(), "  must be an Enum"));
            }
            try
            {
                if (Enum.TryParse<T>(inString, ignoreCase, out t).Equals(false) & throwException)
                {
                    throw new InvalidOperationException("Invalid Cast");
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Invalid Cast", exception);
            }
            return t;
        }

        public static T ParseEnum<T>(int input, bool throwException = true)
        where T : struct
        {
            return EnumHelper.ParseEnum<T>(input, default(T), throwException);
        }

        public static T ParseEnum<T>(int input, T defaultValue, bool throwException = false)
        where T : struct
        {
            T obj = defaultValue;
            if (typeof(T).IsEnum.Equals(false))
            {
                throw new InvalidOperationException(String.Concat("Invalid Enum Type. ", typeof(T).ToString(), "  must be an Enum"));
            }
            if (Enum.IsDefined(typeof(T), input))
            {
                obj = (T)Enum.ToObject(typeof(T), input);
            }
            else if (throwException)
            {
                throw new InvalidOperationException("Invalid Cast");
            }
            return obj;
        }
    }
}