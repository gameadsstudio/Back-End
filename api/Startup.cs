using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using api.Business.Advertisements;
using api.Business.AdContainer;
using api.Business.Tag;
using api.Business.User;
using api.Business.Organization;
using api.Business.Game;
using api.Business.Media;
using api.Business.Version;
using api.Business.Campaign;
using api.Business.MediaQuery;
using api.Contexts;
using api.Enums.User;
using api.Helpers;
using api.Mappings;
using api.Middlewares;
using api.Models.User;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("GAS_SECRET") ?? "secret");
            services.AddControllers();

            services.AddDbContext<ApiContext>(
                p => p.UseNpgsql(
                        $"Host={Environment.GetEnvironmentVariable("GAS_DATABASE_SERVER")};Port=5432;Database={Environment.GetEnvironmentVariable("GAS_POSTGRES_DB")};Username={Environment.GetEnvironmentVariable("GAS_POSTGRES_USER")};Password={Environment.GetEnvironmentVariable("GAS_POSTGRES_PASSWORD")};")
                    .UseSnakeCaseNamingConvention());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Game Ads Studio API", Version = "v1"});
            });
            services.AddSingleton(
                new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); }).CreateMapper());

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnTokenValidated = context =>
                        {
                            var dbcontext = context.HttpContext.RequestServices.GetRequiredService<ApiContext>();
                            var principal = context.Principal;

                            var claim = principal?.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

                            if (claim == null || dbcontext.User.SingleOrDefault(a => a.Id == new Guid(claim.Value)) ==
                                null)
                            {
                                context.Fail("Invalid token");
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdmin",
                    policy => policy.RequireRole(UserRole.Admin.ToString()));
            });
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(opts => { opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

            // Configure max request size. Default is 30 MB
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize =
                    long.Parse(Environment.GetEnvironmentVariable("GAS_FILE_MAX_SIZE") ??
                                   int.MaxValue.ToString()); });

            services.AddHttpContextAccessor();

            // Business Logic
            services.AddScoped<IUserBusinessLogic, UserBusinessLogic>();
			services.AddScoped<IUserAuthServiceBusinessLogic, UserAuthServiceBusinessLogic>();
            services.AddScoped<ITagBusinessLogic, TagBusinessLogic>();
            services.AddScoped<IAdvertisementBusinessLogic, AdvertisementBusinessLogic>();
            services.AddScoped<IOrganizationBusinessLogic, OrganizationBusinessLogic>();
            services.AddScoped<IAdContainerBusinessLogic, AdContainerBusinessLogic>();
            services.AddScoped<IGameBusinessLogic, GameBusinessLogic>();
            services.AddScoped<IVersionBusinessLogic, VersionBusinessLogic>();
            services.AddScoped<ICampaignBusinessLogic, CampaignBusinessLogic>();
            services.AddScoped<IMediaBusinessLogic, MediaBusinessLogic>();
            services.AddScoped<IMediaQueryBusinessLogic, MediaQueryBusinessLogic>();
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
            CreateAdmin(context);

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Game Ads Studio API v1");
                c.RoutePrefix = "documentation";
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void CreateAdmin(ApiContext context)
        {
            // Creating default admin user
            var admin = context.User.FirstOrDefault(user =>
                user.Username == (Environment.GetEnvironmentVariable("GAS_ADMIN_NAME") ?? "admin"));
            if (admin == null)
            {
                context.User.Add(new UserModel()
                {
                    Id = Guid.NewGuid(),
                    Username = Environment.GetEnvironmentVariable("GAS_ADMIN_NAME") ?? "admin",
                    Email =
                        Environment.GetEnvironmentVariable("GAS_ADMIN_EMAIL") ?? "contact@gameadsstudio.com",
                    Role = UserRole.Admin,
                    Password = HashHelper.HashPassword(
                        Environment.GetEnvironmentVariable("GAS_ADMIN_PASSWORD") ?? "password"),
                    FirstName = "",
                    LastName = "",
                    Type = UserType.Admin
                });
            }
            context.SaveChanges();
        }
    }
}
