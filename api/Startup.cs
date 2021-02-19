using System;
using api.Configuration;
using api.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace api
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
            services.Configure<AppSettings>(Configuration.GetSection("Application"));
            services.AddControllers();
            services.AddDbContext<ApiContext>(p =>
                p.UseNpgsql(
                    $"Server={Environment.GetEnvironmentVariable("GAS_DATABASE_SERVER")};Port=5432;Database={Environment.GetEnvironmentVariable("GAS_POSTGRES_DB")};Uid={Environment.GetEnvironmentVariable("GAS_POSGRES_USER")};Pwd={Environment.GetEnvironmentVariable("GAS_POSTGRES_PASSWORD")};"));
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Game Ads Studio API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApiContext context)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            // Auto migrate database on startup
            context.Database.Migrate();

            app.UseCors(builder => builder
            	.AllowAnyOrigin()
            	.AllowAnyMethod()
            	.AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game Ads Studio API v1");
                c.RoutePrefix = "documentation";
            });
        }
    }
}
