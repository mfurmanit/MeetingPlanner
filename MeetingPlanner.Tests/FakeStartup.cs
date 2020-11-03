using System;
using MeetingPlanner.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MeetingPlanner.Tests
{
    public class FakeStartup : Startup
    {
        public FakeStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                
                loggerFactory.CreateLogger<FakeStartup>()
                    .LogInformation($"Test Database Connection String is {dbContext.Database.GetDbConnection().ConnectionString}");

                if (!dbContext.Database.GetDbConnection().ConnectionString.ToLower().Contains("planner_tests"))
                {
                    throw new Exception("Indicated Database Connection String is wrong! Check if you are not using production database!");
                }

                dbContext.Database.EnsureCreated();
            }
        }
    }
}
