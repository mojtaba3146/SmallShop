using Autofac;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using SmallShop.Infrastructure.Application;
using SmallShop.Persistence.EF;
using SmallShop.Persistence.EF.Categories;
using SmallShop.RestApi.Configs.BackgroundServices;
using SmallShop.Services.Categories;

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
                .WithParameter("connectionString", Configuration["ConnectionString"])
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SmallShop.RestApi", Version = "v1" });
            });

            services.AddHostedService<SeedDataGoods>();

            services.AddHealthChecks()
                .AddSqlServer(Configuration["ConnectionString"]!);
                
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseAuthorization();

            app.UseRouting().UseEndpoints(config =>
            {
                config.MapHealthChecks("/health/check", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
