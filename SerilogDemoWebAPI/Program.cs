using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace SerilogDemoWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogger();
            Log.Information("Application Started");
            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });

        public static void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(
                outputTemplate: "[{Timestamp:G} {Level:u3}] {MachineName}<{ThreadId}> {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(@"log.txt", 
                outputTemplate: "[{Timestamp:o} {Level:u3}] {MachineName}<{ThreadId}> {Message:lj}{NewLine}{Exception}")
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .CreateLogger();
        }
    }
}
