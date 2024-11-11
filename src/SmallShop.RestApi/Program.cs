using Autofac.Extensions.DependencyInjection;
using k8s.KubeConfigModels;
using Serilog;

namespace SmallShop.RestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Application>();
                })
            .UseSerilog((context, configuration) =>
              configuration.ReadFrom.Configuration(context.Configuration));
        //.ConfigureLogging(logging =>
        //{
        //    logging.ClearProviders();
        //    logging.AddConsole();
        //    logging.SetMinimumLevel(LogLevel.Information);
        //    logging.AddFile("logging/log.txt");
        //});

    }
}
