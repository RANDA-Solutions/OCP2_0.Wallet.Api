using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenCredentialPublisher.Data.Settings;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.Settings;
using OpenCredentialPublisher.Services.Implementations;

namespace OpenCredentialPublisher.DependencyInjection
{
    public static class Provider
    {
        public static IHostBuilder GetHost()
            => Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => GetServiceCollection(context, services));
                

        public static IServiceCollection GetServiceCollection(HostBuilderContext context, IServiceCollection services = null)
        {
               
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            services ??= new ServiceCollection();
            services.AddLogging();
            services.AddHttpClient(Microsoft.Extensions.Options.Options.DefaultName);
            services.AddMemoryCache();


            var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<WalletDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    sql => {
                        sql.EnableRetryOnFailure(5);
                    });

            });


            services.AddSingleton<HostSettings>(sp => context.Configuration.GetSection(nameof(HostSettings)).Get<HostSettings>());
            services.AddSingleton<MailSettings>(sp => context.Configuration.GetSection(nameof(MailSettings)).Get<MailSettings>());
            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<LogHttpClientService>();
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            services.AddMvc();
            return services;
        }
    }
}
