using System;
using System.Collections.Generic;
using System.Text;

namespace CacheDecorator.Common
{
    public class UnmatchedStringValueException : Exception
    {
        public UnmatchedStringValueException(string value, Type type) : base(String.Concat(new String[] { "String does not match to any value of the specified Enum. Attempted to Parse ", value, " into an Enum of type ", type.Name, "." }))
        {
        }
    }
}