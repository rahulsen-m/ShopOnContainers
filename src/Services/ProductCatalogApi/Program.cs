using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductCatalogApi.Data;
using System;

namespace ProductCatalogApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            // Get service collection from the host object
            using (var scope = host.Services.CreateScope())
            {
                // Get all the services registered in the dependency injection container
                var services = scope.ServiceProvider;

                try
                {
                    // Register the catalog context 
                    var context = services.GetRequiredService<CatalogContext>();
                    CatalogSeed.SeedAsync(context).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }

            host.Run();

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
