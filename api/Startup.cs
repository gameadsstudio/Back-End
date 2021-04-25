using System;
using System.Text;
using api.Configuration;
using api.Contexts;
using api.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("GAS_SECRET")!);
            
            services.Configure<AppSettings>(Configuration.GetSection("Application"));
            services.AddControllers();
            services.AddDbContext<ApiContext>(p =>
                p.UseNpgsql(
                    $"Host={Environment.GetEnvironmentVariable("GAS_DATABASE_SERVER")};Port=5432;Database={Environment.GetEnvironmentVariable("GAS_POSTGRES_DB")};Username={Environment.GetEnvironmentVariable("GAS_POSTGRES_USER")};Password={Environment.GetEnvironmentVariable("GAS_POSTGRES_PASSWORD")};")
                    .UseSnakeCaseNamingConvention()
                    );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Game Ads Studio API", Version = "v1" });
            });
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            }).CreateMapper());
            
            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApiContext context)
        {
            if (env.IsDevelopment())
            {
                // Delete the whole database each time the API will be launched
                context.Database.EnsureDeleted();
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

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game Ads Studio API v1");
                c.RoutePrefix = "documentation";
            });
        }
    }
}