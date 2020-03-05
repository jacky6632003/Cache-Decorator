using System;
using CacheDecorator.Common.Caching;
using CacheDecorator.Common.Settings;
using CacheDecorator.Repository.Decorators.MemoryCache;
using CacheDecorator.Repository.Decorators.Redis;
using CacheDecorator.Repository.Helper;
using CacheDecorator.Repository.Implement;
using CacheDecorator.Repository.Interface;
using CacheDecorator.Service.Implement;
using CacheDecorator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

using Sample.Common.Settings;

namespace CacheDecorator.Infrastructure.Dependency
{
    /// <summary>
    /// Class DependencyInjectionExtensions.
    /// </summary>
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Adds the dendency injection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="systemSettings">The systemSettings.</param>
        /// <param name="cacheDecoratorSettings">The cacheDecoratorSettings.</param>

        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddDendencyInjection(this IServiceCollection services,
                                                              SystemSettings systemSettings,
                                                              CacheDecoratorSettings cacheDecoratorSettings
                                                              )
        {
            // SystemSettings
            services.AddSingleton(x => systemSettings);

            // ISystemSettingsProvider
            services.AddSingleton<ISystemSettingsProvider>(x => new SystemSettingsProvider(systemSettings));

            // ICacheDecoratorSettingsProvider
            services.AddSingleton<ICacheDecoratorSettingsProvider>(x => new CacheDecoratorSettingsProvider(cacheDecoratorSettings));

            // MemoryCache
            services.AddSingleton<IMemoryCache>(x => new MemoryCache(new MemoryCacheOptions()));

            // CacheProviderResolver
            services.AddSingleton<ICacheProviderResolver, CacheProviderResolver>();

            // IHttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Repositories
            services.AddScoped<IFooRepository, FooRepository>()
                    .Decorate<IFooRepository, RedisFooRepository>()
                    .Decorate<IFooRepository, CachedFooRepository>();

            // Services
            services.AddScoped<IFooService, FooService>();

            return services;
        }
    }
}