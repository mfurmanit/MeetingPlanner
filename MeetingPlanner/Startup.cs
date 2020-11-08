using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using MeetingPlanner.Data;
using MeetingPlanner.Dto;
using MeetingPlanner.Repositories;
using MeetingPlanner.Models;
using MeetingPlanner.Others.Exceptions;
using MeetingPlanner.Others.Localization;
using MeetingPlanner.Others.Scheduling;
using MeetingPlanner.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace MeetingPlanner
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer().AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddAuthentication().AddIdentityServerJwt();

            services.AddControllersWithViews().AddNewtonsoftJson().AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddRazorPages()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName =
                            new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName ?? string.Empty);
                        return factory.Create("SharedResource", assemblyName.Name);
                    };
                });

            // Configure localization / culture to translate Razor Pages
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("es"),
                        new CultureInfo("pl")
                    };

                    options.DefaultRequestCulture = new RequestCulture(culture: "pl", uiCulture: "pl");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;

                    options.RequestCultureProviders.Clear();
                    var provider = new LocalizationCookieProvider
                    {
                        CookieName = "language"
                    };
                    options.RequestCultureProviders.Insert(0, provider);
                });

            // DI - Repositories
            services.AddScoped<IEventRepository, EventRepository>();

            // DI - Services
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<INotificationService, NotificationService>();

            // E-mail connection metadata
            var connectionMetadata = Configuration.GetSection("ConnectionMetadata").Get<ConnectionMetadata>();
            services.AddSingleton(connectionMetadata);

            // Localization service
            services.AddSingleton<LocalizationService>();

            // Quartz services
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<QuartzHostedService>();

            // Job and schedule for sending notifications
            services.AddSingleton<NotificationsJob>();
            services.AddSingleton(new JobSchedule(
                typeof(NotificationsJob),
                "0 * * ? * *")); // run every minute

            // Adding AutoMapper ( ModelMapper from Utils )
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Logging file
            var loggingFilePath = Configuration.GetSection("LoggingFilePath").Get<string>();
            loggerFactory.AddFile(loggingFilePath);

            if (env.IsDevelopment())
            {
                app.ConfigureCustomExceptionMiddleware();
            }
            else
            {
                app.ConfigureCustomExceptionMiddleware();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
                app.UseSpaStaticFiles();

            app.UseRouting();

            // Use localization / culture to translate Razor Pages
            app.UseRequestLocalization(app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);
            
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                    spa.UseAngularCliServer(npmScript: "start");
            });
        }
    }
}