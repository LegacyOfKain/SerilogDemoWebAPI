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
                //you can avoid console for prod applications
                .WriteTo.Console(
                outputTemplate: "[{Timestamp:G} {Level:u3}] {MachineName}<{ThreadId}> {Message:lj}{NewLine}{Exception}")
                //changed file logging to async to unmount the load of logging from the thread of the main app
                .WriteTo.Async(a =>
                {
                    a.PersistentFile("log.txt", 
                        fileSizeLimitBytes:4096 ,
                        rollOnFileSizeLimit : true, 
                        retainedFileCountLimit : 3,
                        preserveLogFilename: true); // <<<<<
                })
                /*
                .WriteTo.File(@"log.txt", 
                outputTemplate: "[{Timestamp:o} {Level:u3}] {MachineName}<{ThreadId}> {Message:lj}{NewLine}{Exception}")
                */
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .CreateLogger();
        }
    }
}
