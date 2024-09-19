using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using OpenCredentialPublisher.Data.Options;
using Serilog;

namespace OpenCredentialPublisher.Wallet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Open Credential Publisher - Learning & Employment Wallet";

            try
            {
                var host = CreateHostBuilder(args).Build();
                Log.Information("Starting host...");
                host.Run();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                Trace.WriteLine(e.ToString());
                Log.Fatal(e, "Host terminated unexpectedly.");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopmentOrLocalhost())
                        builder.AddUserSecrets<Program>();
                    var configuration = builder.Build();
                    builder.AddAzureAppConfiguration(config =>
                    {
                        var azureAppConfiguration = configuration.GetRequiredSection(AzureAppConfigConfiguration.SectionName).Get<AzureAppConfigConfiguration>();
                        if (azureAppConfiguration == null)
                            throw new Exception("Azure App Configuration is not configured");
                        config.Connect(azureAppConfiguration.ConnectionString)
                            .Select(KeyFilter.Any, LabelFilter.Null)
                            .Select(KeyFilter.Any, azureAppConfiguration.Label);
                    }, true);
                    // do this again so user secrets override AZ config
                    if (context.HostingEnvironment.IsDevelopmentOrLocalhost())
                        builder.AddUserSecrets<Program>();
                });
            builder.UseSerilog();
            return builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
#if IIS
                webBuilder.UseIIS();
#endif
            });
        }
    }
}
