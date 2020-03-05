using System;

namespace CacheDecorator.Repository.Decorators
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