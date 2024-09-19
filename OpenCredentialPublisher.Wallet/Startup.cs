using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using OpenCredentialPublisher.Data.Custom.Contexts;
using OpenCredentialPublisher.Data.Custom.Options;
using OpenCredentialPublisher.Data.Custom.Settings;
using OpenCredentialPublisher.Data.Models;
using OpenCredentialPublisher.Data.Options;
using OpenCredentialPublisher.Data.Settings;
using OpenCredentialPublisher.DependencyInjection;
using OpenCredentialPublisher.Services.Implementations;
using OpenCredentialPublisher.Wallet.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace OpenCredentialPublisher.Wallet
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services.Configure<AzureBlobOptions>(Configuration.GetSection(AzureBlobOptions.Section));
            services.Configure<PublicBlobOptions>(Configuration.GetSection(PublicBlobOptions.Section));
            services.Configure<SourcesSettingsOptions>(Configuration.GetSection(SourcesSettingsOptions.Section));
            services.Configure<DevSettingsOptions>(Configuration.GetSection(DevSettingsOptions.Section));


            var siteSettingsSection = Configuration.GetSection(SiteSettingsOptions.Section);
            services.Configure<SiteSettingsOptions>(siteSettingsSection);
            var siteSettingsOptions = siteSettingsSection.Get<SiteSettingsOptions>();

            services.Configure<HostSettings>(Configuration.GetSection(nameof(HostSettings)));
            var keyVaultSection = Configuration.GetSection(nameof(KeyVaultOptions));
            services.Configure<KeyVaultOptions>(keyVaultSection);

            if (Environment.IsDevelopmentOrLocalhost())
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
                    .MinimumLevel.Override("System", LogEventLevel.Debug)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Debug)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                    .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                    .CreateLogger();
            }

            services.AddControllers(options =>
            {
                options.Filters.Add<ValidatorActionFilter>(); // If model invalid, throw model invalid exception so middleware can catch it...
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    context.HttpContext.Request.EnableBuffering();
                    var reqBody = context.HttpContext.Request.Body;

                    // Read the request body asynchronously
                    var body = ReadRequestBodyAsync(reqBody).GetAwaiter().GetResult();

                    var message = $"(ModelBinding) There were {context.ModelState.Select(ms => ms.Value.Errors.Count).Sum()} validation error(s). [";
                    foreach (var kvp in context.ModelState)
                    {
                        var propertyMessage = kvp.Key + ":" + string.Join(';', kvp.Value.Errors.Select(e => e.ErrorMessage));
                        message += " " + propertyMessage;
                    }
                    message += "]";
                    message += System.Environment.NewLine;
                    message += body;

                    throw new ApiModelValidationException(context.ModelState, message);
                };
            });

            //services.AddHostedService<ScheduledFeedService>();

            if (Environment.IsDevelopmentOrLocalhost())
            {
                // Accept any server certificate
                services.AddHttpClient(ClrHttpClient.Default, _ => { })
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback =
                            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    });
            }
            else
            {
                services.AddHttpClient(ClrHttpClient.Default);
            }

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<WalletDbContext>(options =>
            {
                options.UseSqlServer(connectionString,
                    sql =>
                    {
                        sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        sql.EnableRetryOnFailure(5);
                    });
            });

            if (Environment.IsDevelopment())
                services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddMemoryCache();
            services.AddHttpContextAccessor();

            services.AddSingleton(_ => Configuration.GetSection(nameof(HostSettings)).Get<HostSettings>());
            services.AddSingleton(_ => Configuration.GetSection(nameof(MailSettings)).Get<MailSettings>());

            services.AddTransient<IEmailSender, EmailService>();


            RegisterServices.AppServiceRegistration(services); //<=====================================================================================

            services.AddCors(options => CorsConfig.CorsOptions(options, siteSettingsOptions));

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext!);
            });

            // ConfigureModelBindingExceptionHandling(services);

            services.AddIdentity<ApplicationUser, IdentityRole>(
                    options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireUppercase = true;
                        options.Password.RequireNonAlphanumeric = true;
                        options.Password.RequiredLength = 6;
                        options.Password.RequiredUniqueChars = 0;
                    })
                .AddEntityFrameworkStores<WalletDbContext>()
                .AddDefaultTokenProviders();

            var jwtConfigurationSection = Configuration.GetSection(JwtConfiguration.SectionName);
            var jwtConfiguration = jwtConfigurationSection.Get<JwtConfiguration>();
            if (jwtConfiguration == null)
                throw new ArgumentNullException(nameof(jwtConfiguration));

            services.Configure<JwtConfiguration>(jwtConfigurationSection);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.RequireHttpsMetadata = true;
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidAudience = jwtConfiguration.ValidAudience,
                            ValidIssuer = jwtConfiguration.ValidIssuer,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret)), // swap this out for azure keyvault key
                        };
                    });

            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Path.Value!.StartsWith("/hubs")

                            && context.Request.Headers.TryGetValue("Bearer", out StringValues token)
                        )
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = _ => Task.CompletedTask
                };
            }
            );
            services.ConfigureApplicationCookie(options =>
            {
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromSeconds(siteSettingsOptions.SessionTimeout);
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Unspecified;
                options.LoginPath = "/access/login";
                options.LogoutPath = "/access/logout";
                options.AccessDeniedPath = "/error";
            });

            services.AddControllers();

            services.AddSignalR();
            //services.AddMediatR(typeof(Startup), typeof(EPMSResponseHandler));
        }


        private static async Task<string> ReadRequestBodyAsync(System.IO.Stream reqBody)
        {
            reqBody.Seek(0, System.IO.SeekOrigin.Begin); // Ensure stream position is at the beginning
            using (var sr = new System.IO.StreamReader(reqBody, leaveOpen: true))
            {
                var body = await sr.ReadToEndAsync();
                reqBody.Seek(0, System.IO.SeekOrigin.Begin); // Reset the stream position to allow it to be read again
                return body;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IServiceProvider services)
        {
            InitializeDatabase(app);

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                CheckConsentNeeded = _ => true,
                MinimumSameSitePolicy = SameSiteMode.None
            });
            app.UseForwardedHeaders();
            var basePath = Configuration[Wallet.Configuration.BasePath];
            if (!string.IsNullOrEmpty(basePath))
            {
                Log.Debug($"Found base path '{basePath}'.");

                app.UsePathBase(basePath);
                app.Use((context, next) =>
                {
                    if (string.IsNullOrEmpty(context.Request.PathBase))
                    {
                        context.Request.PathBase = new PathString(basePath);
                    }

                    return next();
                });
            }
            if (!Environment.IsProduction())
                app.EnableRequestBodyRewind();

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseHttpsRedirection(); mdoTemp
            // may need additional filters here for connecting, etc. be on the lookout for issues...
            app.UseWhen(context => !context.Request.Path.Value!.ToLower().Contains("export"), appBuilder =>
            {
                appBuilder.UseApiErrorHandlingMiddleware();
            });


            app.UseRouting();
            app.UseCors(CorsConfig.PolicyName);
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CreateRoles(services).Wait();
        }
        //private async Task ConfigureModelBindingExceptionHandling(IServiceCollection services)
        //{
        //    services.Configure<ApiBehaviorOptions>(options =>
        //    {
        //        options.InvalidModelStateResponseFactory = actionContext =>
        //        {
        //            ValidationProblemDetails error = actionContext.ModelState
        //                .Where(e => e.Value.Errors.Count > 0)
        //                .Select(e => new ValidationProblemDetails(actionContext.ModelState)).FirstOrDefault();

        //            // Here you can add logging to you log file or to your Application Insights.
        //            // For example, using Serilog:
        //            // Log.Error($"{{@RequestPath}} received invalid message format: {{@Exception}}", 
        //            //   actionContext.HttpContext.Request.Path.Value, 
        //            //   error.Errors.Values);
        //            var uri = actionContext.HttpContext.Request.Path;

        //            return new BadRequestObjectResult(error);
        //        };
        //    });
        //}
        private void InitializeDatabase(IApplicationBuilder app)
        {
            // Don't allow auto-migration in production environment
            // Generate SQL scripts for review and deploy coordination
            if (Environment.IsProduction()) return;

            //using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            //var walletDbContext = serviceScope.ServiceProvider.GetRequiredService<WalletDbContext>();
            //walletDbContext.Database.Migrate();
        }
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "System.Administrator" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //Here you could create a super user who will maintain the web app

        }
    }

    public static class WebHostExtensions
    {
        public static bool IsDevelopmentOrLocalhost(this IHostEnvironment environment)
        {
            return environment.IsDevelopment() || environment.IsEnvironment(Constants.Localhost);
        }
    }

    public static class Configuration
    {
        /// <summary>
        /// </summary>
        public static string BasePath = "BasePath";
    }


}
