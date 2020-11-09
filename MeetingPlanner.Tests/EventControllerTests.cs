using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MeetingPlanner.Data;
using MeetingPlanner.Dto;
using MeetingPlanner.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace MeetingPlanner.Tests
{
    public class EventControllerTests : IClassFixture<CustomWebApplicationFactory<FakeStartup>>, IDisposable
    {
        private readonly WebApplicationFactory<FakeStartup> _factory;

        public EventControllerTests(CustomWebApplicationFactory<FakeStartup> factory)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.Test.json");

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services => services
                    .AddDbContext<ApplicationDbContext>(options => options
                        .UseInMemoryDatabase("MeetingPlannerTestDatabase")));

                builder.UseSolutionRelativeContentRoot("MeetingPlanner");

                builder.ConfigureAppConfiguration((conf, confBuilder) => confBuilder.AddJsonFile(configPath));

                builder.ConfigureTestServices(services => services.AddMvc().AddApplicationPart(typeof(Startup).Assembly));
            });
        }

        public void Dispose()
        {
            using var scope = _factory.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Notifications.RemoveRange(context.Notifications);
            context.Events.RemoveRange(context.Events);
            context.SaveChanges();
        }

        [Theory]
        [InlineData("/api/events/global?date=2020-11-03")]
        public async Task GlobalEventsGetEndpoint_ShouldReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            using var scope = _factory.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(0, context.Events.Count());
        }

        [Theory]
        [InlineData("/api/events/global?date=2020-11-03")]
        public async Task GlobalEventsGetEndpoint_ShouldReturnBothEventsFromDatabase(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            using var scope = _factory.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var firstDate = new DateTime(2020, 11, 15);
            var secondDate = new DateTime(2020, 11, 16);

            await context.Events.AddAsync(EventFixtures.SomeEvent().Title("First event").Date(firstDate).Build());
            await context.Events.AddAsync(EventFixtures.SomeEvent().Title("Second event").Date(secondDate).Build());
            await context.SaveChangesAsync();

            // Act
            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<EventResponse>>(responseContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            Assert.Equal(2, events.Count);

            var firstEvent = events.SingleOrDefault(eventResponse => eventResponse.Title.Equals("First event"));
            var secondEvent = events.SingleOrDefault(eventResponse => eventResponse.Title.Equals("Second event"));

            Assert.NotNull(firstEvent);
            Assert.NotNull(secondEvent);

            Assert.Equal(firstDate, firstEvent.Date);
            Assert.Equal(secondDate, secondEvent.Date);
        }

        [Theory]
        [InlineData("/api/events/global?date=2021-11-03")]
        public async Task GlobalEventsGetEndpoint_ShouldNotReturnEventsFromDatabaseDueToDateQueryParam(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            using var scope = _factory.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Events.AddAsync(EventFixtures.SomeEvent().Build());
            await context.Events.AddAsync(EventFixtures.SomeEvent().Build());
            await context.SaveChangesAsync();

            // Act
            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<EventResponse>>(responseContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.Empty(events);
        }

        [Theory]
        [InlineData("/api/events")]
        public async Task EventsPostEndpoint_ShouldCreateGlobalEvent(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            var requestObject = EventFixtures.SomeEventRequest().Build();
            var json = JsonConvert.SerializeObject(requestObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<EventResponse>(responseContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.NotEqual(responseObject.Id, Guid.Empty);
            Assert.Equal(responseObject.Title, requestObject.Title);
            Assert.Empty(responseObject.Notifications);

            using var scope = _factory.Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(1, context.Events.Count());
        }
    }
}
