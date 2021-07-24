using System.Collections.Generic;
using AspNetCoreRateLimit;
using ICEWeatherExercise.Contracts;
using ICEWeatherExercise.Contracts.Storages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ICEWeatherExercise.Core;
using ICEWeatherExercise.Core.Storages;
using ICEWeatherExercise.Middlewares;
using Microsoft.Extensions.Configuration;

namespace ICEWeatherExercise
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();
            services.AddScoped<IWeatherForecastProvider, WeatherForecastProvider>();
            services.AddScoped<IRemoteFileStorage, RemoteFileStorage>();
            services.AddScoped<ILocalFileStorage, LocalFileStorage>();
            services.AddScoped<IWeatherForecastFileParser, WeatherForecastFileParser>();
            
            // Requests limit configuration
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseMiddleware<ErrorLoggerMiddleware>();

            app.UseIpRateLimiting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
