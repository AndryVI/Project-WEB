using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebH
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 //Добавление файлов .json
                 .ConfigureAppConfiguration(config =>
                 {
                     config.AddJsonFile("appsettings.Task-5.json", optional: true)  
                     .AddJsonFile("appsettings.Task-1.json", optional: true, reloadOnChange: true)
                     .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);
                 })

                // В секции Logging файла appsettigns.json определены настройки системы логирования
                .ConfigureLogging((context, builder) =>
                {
                    // Данный код существует в методе CreateDefaultBuilder 
                    builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                    builder.AddConsole();
                    // NuGet Package NetEscapades.Extensions.Logging.RollingFile
                    builder.AddFile(); 
                })

                //Добавление Seq
                .ConfigureLogging(builder => builder.AddSeq())

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
