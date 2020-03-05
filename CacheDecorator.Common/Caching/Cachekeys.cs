using System;
using System.Collections.Generic;
using System.Text;

namespace CacheDecorator.Common.Caching
{
    public class Cachekeys
    {
        public class Foo
        {
            /// <summary>
            /// Foo::Exists::{FooId}
            /// </summary>
            public static string Exists => "Foo::Exists::{0}";

            /// <summary>
            /// Foo::Get::{FooId}
            /// </summary>
            public static string Get => "Foo::Get::{0}";
        }
    }
}