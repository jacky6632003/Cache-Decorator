using System;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace CacheDecorator.Common
{
    public static class StringExtensions
    {
        public static string EraseHtmlTag(this string value)
        {
            if (value.IsNullOrWhiteSpace())
            {
                return String.Empty;
            }
            string str = value.Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n").Replace("<BR>", "\n").Replace("<BR/>", "\n").Replace("<BR />", "\n").Replace("&nbsp;", String.Empty);
            str = (new Regex("\\<[^\\>]*\\>")).Replace(str, String.Empty);
            if (str.IsNullOrWhiteSpace())
            {
                return String.Empty;
            }
            return str.Trim();
        }

        public static string HtmlDecode(this string s)
        {
            return WebUtility.HtmlDecode(s);
        }

        public static string HtmlEncode(this string s)
        {
            return WebUtility.HtmlEncode(s);
        }

        public static bool IsMatch(this string s, string regEx)
        {
            return (new Regex(regEx)).IsMatch(s);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        public static bool IsNumber(this string strNumber)
        {
            string str = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            string str1 = "^([-]|[0-9])[0-9]*$";
            if (strNumber.IsMatch("[^0-9.-]") || strNumber.IsMatch("[0-9]*[.][0-9]*[.][0-9]*") || strNumber.IsMatch("[0-9]*[-][0-9]*[-][0-9]*"))
            {
                return false;
            }
            return strNumber.IsMatch("({0})|({1})".ToFormat(new Object[] { str, str1 }));
        }

        public static bool IsValidEmailAddress(this string s)
        {
            return (new Regex("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$")).IsMatch(s);
        }

        public static bool IsValidMobilePhone(this string s)
        {
            return (new Regex("^09\\d{8}$")).IsMatch(s);
        }

        public static string Left(this string s, int length)
        {
            length = Math.Max(length, 0);
            if (s.Length <= length)
            {
                return s;
            }
            return s.Substring(0, length);
        }

        public static string Right(this string s, int length)
        {
            length = Math.Max(length, 0);
            if (s.Length <= length)
            {
                return s;
            }
            return s.Substring(s.Length - length, length);
        }

        public static bool ToBool(this string value)
        {
            return value.ToBool(false);
        }

        public static bool ToBool(this string value, bool defaultValue = false)
        {
            bool flag;
            if (!Boolean.TryParse(value, out flag))
            {
                return defaultValue;
            }
            return flag;
        }

        public static Decimal ToDecimal(this string value)
        {
            Decimal num = new Decimal();
            if (String.IsNullOrWhiteSpace(value).Equals(false))
            {
                Decimal.TryParse(value, out num);
            }
            return num;
        }

        public static double ToDouble(this string value)
        {
            double num = 0;
            if (String.IsNullOrWhiteSpace(value).Equals(false))
            {
                Double.TryParse(value, out num);
            }
            return num;
        }

        public static string ToFormat(this string format, params object[] args)
        {
            return String.Format(format, args);
        }

        public static Guid ToGuid(this string value)
        {
            Guid empty = Guid.Empty;
            if (String.IsNullOrWhiteSpace(value).Equals(false))
            {
                Guid.TryParse(value, out empty);
            }
            return empty;
        }

        public static int ToInt(this string input)
        {
            return input.ToInt(0);
        }

        public static int ToInt(this string input, int defaultValue = 0)
        {
            int num;
            if (!Int32.TryParse(input, out num))
            {
                return defaultValue;
            }
            return num;
        }

        public static long ToInt16(this string value)
        {
            short num = 0;
            if (String.IsNullOrEmpty(value).Equals(false))
            {
                Int16.TryParse(value, out num);
            }
            return (long)num;
        }

        public static long ToInt32(this string value)
        {
            int num = 0;
            if (String.IsNullOrEmpty(value).Equals(false))
            {
                Int32.TryParse(value, out num);
            }
            return (long)num;
        }

        public static long ToInt64(this string value)
        {
            long num = (long)0;
            if (String.IsNullOrEmpty(value).Equals(false))
            {
                Int64.TryParse(value, out num);
            }
            if (num.Equals((long)0) && !String.IsNullOrEmpty(value) && value.Contains<char>('.'))
            {
                num = Convert.ToInt64(value.ToDouble());
            }
            return num;
        }

        public static string[] ToSplit(this string s, string separator)
        {
            string[] strArray = new String[0];
            if (s.IsNullOrWhiteSpace())
            {
                return strArray;
            }
            return s.Split(separator.ToCharArray());
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (value.IsNullOrWhiteSpace())
            {
                return String.Empty;
            }
            if (value.Trim().Length <= maxLength)
            {
                return value.Trim();
            }
            return value.Trim().Substring(0, maxLength);
        }

        public static string UrlDecode(this string s)
        {
            return WebUtility.UrlDecode(s);
        }

        public static string UrlEncode(this string s)
        {
            return WebUtility.UrlEncode(s);
        }
    }
}