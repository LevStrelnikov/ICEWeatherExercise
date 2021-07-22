using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ICEWeatherExercise.Core;
using ICEWeatherExercise.Core.Contracts;
using ICEWeatherExercise.Core.Storages;
using ICEWeatherExercise.Middlewares;

namespace ICEWeatherExercise
{
    public class Startup
    {
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


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
