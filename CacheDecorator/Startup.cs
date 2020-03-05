using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CacheDecorator.Common.Settings;
using CacheDecorator.Infrastructure.Dependency;
using CacheDecorator.Infrastructure.Mappings;
using CacheDecorator.Repository.Helper;
using CacheDecorator.Service.Mapping;
using CoreProfiler.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CacheDecorator
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private string WLDOonnection { get; set; }

        private string MySQLConnection { get; set; }

        private SystemSettings SystemSettings { get; set; }

        private CacheDecoratorSettings CacheDecoratorSettings { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // database connection - Wldo
            this.WLDOonnection = this.Configuration.GetConnectionString("WLDO");

            // database connection - Mysql
            this.MySQLConnection = this.Configuration.GetConnectionString("MySQL");

            //DB
            services.AddScoped<IDatabaseHelper>(x => new DatabaseHelper(WLDOonnection, MySQLConnection));

            //Dendency Injection

            // 系統設定
            var systemSettings = new SystemSettings();
            this.Configuration.GetSection("SystemSettings").Bind(systemSettings);
            this.SystemSettings = systemSettings;

            // 快取裝飾者設定
            var cacheDecoratorSettings = new CacheDecoratorSettings();
            this.Configuration.GetSection("CacheDecoratorSettings").Bind(cacheDecoratorSettings);
            this.CacheDecoratorSettings = cacheDecoratorSettings;

            services.AddMemoryCache();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6379";
                options.InstanceName = "CacheDecorator_Sample";
            });

            services.AddAutoMapper
            (
                typeof(ServiceMappingProfile).Assembly,
                typeof(WebApplicationMappingProfile).Assembly
            );

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // DendencyInjection
            services.AddDendencyInjection(this.SystemSettings, this.CacheDecoratorSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCoreProfiler(true);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}