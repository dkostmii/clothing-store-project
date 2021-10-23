using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using clothing_store_api.Data;
using clothing_store_api.Services;

namespace clothing_store_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var initContext = services.GetRequiredService<InitContext>();
                var authService = services.GetRequiredService<AuthService>();
                var productsService = services.GetRequiredService<ProductsService>();
                var imagesService = services.GetRequiredService<ImagesService>();


                var logger = services.GetRequiredService<ILogger<Program>>();
                var config = services.GetRequiredService<IConfiguration>();

                try
                {
                    logger.LogInformation("Attempting to seed database");
                    DbInitializer.Initialize(logger, initContext);
                    DbInitializer.AdvancedInit(config, initContext, authService, productsService, imagesService, logger).Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
