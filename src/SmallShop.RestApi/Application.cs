using Autofac;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using SmallShop.Infrastructure.Application;
using SmallShop.Infrastructure.Jwt;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.RestApi.BackGroundServices;
using SmallShop.RestApi.Configs.BackgroundServices;
using SmallShop.RestApi.Middleware;
using SmallShop.Services.Categories;
using System.Text;

namespace SmallShop.RestApi
{
    public class Application
    {
        public Application(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<EFDataContext>()
                .WithParameter("connectionString", Configuration["ConnectionString"]!)
                 .AsSelf()
                 .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(EFCategoryRepository).Assembly)
                          .AssignableTo<Repository>()
                          .AsImplementedInterfaces()
                          .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(CategoryAppService).Assembly)
                      .AssignableTo<Service>()
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            builder.RegisterType<EFUnitOfWork>()
                .As<UnitOfWork>()
                .InstancePerLifetimeScope();
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmallShop.RestApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "This Moji Site Use Bearer Token To Pass",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            services.AddHostedService<SeedDataGoods>();
            services.AddHostedService<RoleSeedHostedService>();

            services.AddHealthChecks()
                .AddSqlServer(Configuration["ConnectionString"]!);

            services.AddScoped<IJwtService, JwtTokenService>();

            var jwtSettings = new JwtSettings();
            Configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey
                      (Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmallShop.RestApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestLogContextMiddleware>();

            app.UseSerilogRequestLogging();


            app.UseEndpoints(config =>
            {
                config.MapHealthChecks("/health/check", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            app.UseEndpoints(config =>
            {
                config.MapGet("/mojtaba",
                    () => "mojtaba khoshnam wrote this app");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
