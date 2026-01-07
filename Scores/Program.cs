using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Scores.Factories;
using Scores.Interfaces;
using Scores.Models;
using Scores.Strategies;

namespace Scores
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                Log.Information("Application starting...");

                var host = CreateHostBuilder(args).Build();

                var csvHelper = host.Services.GetRequiredService<ICSVHelper>();
                var mapper = host.Services.GetRequiredService<IStudentMapper>();
                var jsonHelper = host.Services.GetRequiredService<IJsonHelper>();
                var appSettings = host.Services.GetRequiredService<AppSettings>();
                var logger = host.Services.GetRequiredService<ILogger<Program>>();

                string inputFilePath = args.Length > 0 ? args[0] : appSettings.DefaultInputPath;
                string outputFilePath = args.Length > 1 ? args[1] : 
                    (appSettings.EnableAutoSave ? appSettings.DefaultOutputPath : null);

                logger.LogInformation("Starting CSV processing from: {InputPath}", inputFilePath);

                var rows = await csvHelper.ReadCSVAsync(inputFilePath);
                var studentSubjects = await mapper.MapAndSortScoresAsync(rows);
                string json = jsonHelper.ConvertToJson(studentSubjects);
                jsonHelper.DisplayJson(json);

                if (!string.IsNullOrEmpty(outputFilePath))
                {
                    await jsonHelper.SaveToFileAsync(json, outputFilePath);
                }

                logger.LogInformation("Processing completed successfully!");
                Console.WriteLine("\n Processing completed successfully!");

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                
                if (args.Any(a => a.Contains("--verbose")))
                {
                    Console.WriteLine($"Stack Trace:\n{ex.StackTrace}");
                }
                
                return 1;
            }
            finally
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
                Log.CloseAndFlush();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var appSettings = context.Configuration.GetSection("AppSettings").Get<AppSettings>();
                    services.AddSingleton(appSettings ?? new AppSettings());

                    var strategies = new List<KeyValuePair<string, IScoreSortingStrategy>>
                    {
                        new KeyValuePair<string, IScoreSortingStrategy>("English", new EnglishSortingStrategy()),
                        new KeyValuePair<string, IScoreSortingStrategy>("Maths", new MathsSortingStrategy()),
                        new KeyValuePair<string, IScoreSortingStrategy>("Science", new ScienceSortingStrategy())
                    };
                    services.AddSingleton<IEnumerable<KeyValuePair<string, IScoreSortingStrategy>>>(strategies);

                    services.AddTransient<ICSVHelper, CSVHelper>();
                    services.AddTransient<IJsonHelper, JsonHelper>();
                    services.AddTransient<IStudentMapper, StudentMapper>();
                    services.AddSingleton<ISortingStrategyFactory, SortingStrategyFactory>();
                });
    }
}