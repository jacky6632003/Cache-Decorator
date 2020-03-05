using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CacheDecorator.Common
{
    public static class EnumExtensions
    {
        public static TEnum ChangeType<TEnum>(this Enum e)
        where TEnum : IComparable
        {
            return e.SetFlags<TEnum>(default(TEnum), false);
        }

        public static string EnumDescription(this Enum value)
        {
            EnumDescriptionAttribute[] customAttributes = (EnumDescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (customAttributes.Length == 0)
            {
                return value.ToString();
            }
            return customAttributes[0].Description;
        }

        public static IEnumerable<TEnumType> EnumerateValues<TEnumType>()
        {
            return Enum.GetValues(typeof(TEnumType)).Cast<TEnumType>();
        }

        public static IEnumerable<string> GetAllStringValues(this Enum value)
        {
            return
                from pair in value.GetStringValuesWithPreferences()
                select pair.StringValue;
        }

        public static T GetAttributeOfType<T>(this Enum enumVal)
        where T : Attribute
        {
            object[] customAttributes = enumVal.GetType().GetMember(enumVal.ToString())[0].GetCustomAttributes(typeof(T), false);
            if (customAttributes.Length == 0)
            {
                return default(T);
            }
            return (T)customAttributes[0];
        }

        private static IEnumerable<string> GetPreferredStringValues(this Enum value)
        {
            return
                from pair in value.GetStringValuesWithPreferences()
                where pair.Preferred
                select pair.StringValue;
        }

        public static string GetStringValue(this Enum value)
        {
            IEnumerable<string> list = value.GetPreferredStringValues().ToList<string>();
            if (list.Any<string>())
            {
                return list.First<string>();
            }
            return value.GetAllStringValues().FirstOrDefault<string>();
        }

        private static IEnumerable<string> GetStringValues<TEnumType>(TEnumType enumValue)
        where TEnumType : struct, IConvertible
        {
            return ((object)enumValue as Enum).GetAllStringValues();
        }

        private static IEnumerable<StringValueAttribute> GetStringValuesWithPreferences(this Enum value)
        {
            return value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(StringValueAttribute), false).Cast<StringValueAttribute>();
        }

        public static bool HasDescription(this Enum value)
        {
            return ((EnumDescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(EnumDescriptionAttribute), false)).Any<EnumDescriptionAttribute>();
        }

        private static TEnum SetFlags<TEnum>(this Enum e, TEnum flags, bool typeCheck = true)
        where TEnum : IComparable
        {
            if (typeCheck && e.GetType() != flags.GetType())
            {
                throw new ArgumentException("Argument is not the same type as this instance.", "flags");
            }
            Type underlyingType = Enum.GetUnderlyingType(typeof(TEnum));
            uint num = Convert.ToUInt32(e);
            Convert.ToUInt32(flags);
            TEnum tEnum = (TEnum)Convert.ChangeType(num, underlyingType);
            if (typeCheck)
            {
                return tEnum;
            }
            Array values = Enum.GetValues(typeof(TEnum));
            TEnum value = (TEnum)values.GetValue(values.Length - 1);
            if (tEnum.CompareTo(value) <= 0)
            {
                return tEnum;
            }
            return value;
        }

        public static T ToEnum<T>(this string input, bool ignoreCase = true, bool throwException = true)
        where T : struct
        {
            return EnumHelper.ParseEnum<T>(input, ignoreCase, throwException);
        }

        public static T ToEnum<T>(this string input, T defaultValue, bool ignoreCase = true, bool throwException = false)
        where T : struct
        {
            return EnumHelper.ParseEnum<T>(input, defaultValue, ignoreCase, throwException);
        }

        public static T ToEnum<T>(this int input, bool throwException = true)
        where T : struct
        {
            return EnumHelper.ParseEnum<T>(input, default(T), throwException);
        }

        public static T ToEnum<T>(this int input, T defaultValue, bool throwException = false)
        where T : struct
        {
            return EnumHelper.ParseEnum<T>(input, defaultValue, throwException);
        }

        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }
    }
}