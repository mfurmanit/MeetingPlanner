using System;
using System.IO;
using System.Linq;
using MeetingPlanner.Data;
using MeetingPlanner.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MeetingPlanner.Tests
{
    public class IntegrationTestBase : IClassFixture<CustomWebApplicationFactory<FakeStartup>>, IDisposable
    {
        private WebApplicationFactory<FakeStartup> _factory;

        public IntegrationTestBase(CustomWebApplicationFactory<FakeStartup> factory)
        {
            _factory = factory;
        }

        public void Dispose()
        {
            using var scope = GetFactory().Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Notifications.RemoveRange(context.Notifications);
            context.Events.RemoveRange(context.Events);
            context.SaveChanges();
        }

        protected WebApplicationFactory<FakeStartup> GetFactory(bool withAuthentication = false,
            bool withUserContext = false)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.Test.json");

            _factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => services
                    .AddDbContext<ApplicationDbContext>(options => options
                        .UseInMemoryDatabase("MeetingPlannerTestDatabase")));

                builder.UseSolutionRelativeContentRoot("MeetingPlanner");

                builder.ConfigureAppConfiguration((conf, confBuilder) => confBuilder.AddJsonFile(configPath));

                builder.ConfigureTestServices(services =>
                {
                    services.AddMvc(options =>
                        {
                            if (withUserContext)
                            {
                                options.Filters.Add(new AllowAnonymousFilter());
                                options.Filters.Add(new FakeUserFilter());
                            }
                        })
                        .AddApplicationPart(typeof(Startup).Assembly);

                    if (withAuthentication)
                    {
                        services.AddAuthentication(FakeUserSettings.Scheme)
                            .AddScheme<AuthenticationSchemeOptions, FakeAuthenticationHandler>(
                                FakeUserSettings.Scheme, options => { });
                    }
                });
            });

            if (withUserContext)
            {
                ConfigureFakeUser();
            }

            return _factory;
        }

        private void ConfigureFakeUser()
        {
            using var scope = _factory.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (context.Users.Any(u => u.Id == FakeUserSettings.Id)) return;

            var user = new ApplicationUser
            {
                ConcurrencyStamp = DateTime.Now.Ticks.ToString(),
                Email = FakeUserSettings.Email,
                EmailConfirmed = true,
                Id = FakeUserSettings.Id
            };

            user.NormalizedEmail = user.Email;
            user.NormalizedUserName = user.Email;
            user.PasswordHash = Guid.NewGuid().ToString();
            user.UserName = user.Email;

            var role = new IdentityRole
            {
                ConcurrencyStamp = DateTime.Now.Ticks.ToString(),
                Id = FakeUserSettings.Role,
                Name = FakeUserSettings.Role
            };

            var userRole = new IdentityUserRole<string> {RoleId = FakeUserSettings.Role, UserId = user.Id};

            context.Users.Add(user);
            context.Roles.Add(role);
            context.UserRoles.Add(userRole);
            context.SaveChanges();
        }
    }
}