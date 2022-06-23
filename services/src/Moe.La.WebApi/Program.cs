using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moe.La.Infrastructure;
using Moe.La.Infrastructure.DbContexts;
using Serilog;
using System;

namespace Moe.La.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("serilog.config.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();


            try
            {
                // Initialize the database.
                var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();

                using (var scope = scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<LaDbContext>();

                    if (db.Database.EnsureCreated())
                    {
                        Log.Information("Initializing Database.");
                        SeedData.Initialize(db, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development);
                        Log.Information("Database initialized successfully.");
                    }
                }

                Log.Information("MOE Legal Affairs Web API is Starting Up.");

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "MOE Legal Affairs Web API failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
