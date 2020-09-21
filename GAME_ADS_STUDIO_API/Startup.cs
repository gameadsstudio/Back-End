using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using GAME_ADS_STUDIO_API.Configuration;
using GAME_ADS_STUDIO_API.Contexts;

namespace GAME_ADS_STUDIO_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }




        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("Application");
            services.Configure<AppSettings>(appSettingsSection);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                // Permet de préciser de la documentation 
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Game Ads Studio API", Version = "v1" });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My simple API V1");
            });
        }
    }
}
