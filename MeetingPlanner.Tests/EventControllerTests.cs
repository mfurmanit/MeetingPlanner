using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MeetingPlanner.Dto;
using MeetingPlanner.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace MeetingPlanner.Tests
{
    public class EventControllerTests : IClassFixture<CustomWebApplicationFactory<FakeStartup>>
    {
        private readonly WebApplicationFactory<FakeStartup> _factory;

        public EventControllerTests(CustomWebApplicationFactory<FakeStartup> factory)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSolutionRelativeContentRoot("MeetingPlanner");

                builder.ConfigureAppConfiguration((conf, confBuilder) => confBuilder.AddJsonFile(configPath));

                builder.ConfigureTestServices(services => services.AddMvc().AddApplicationPart(typeof(Startup).Assembly));
            });
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
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/api/events")]
        public async Task EventsPostEndpoint_ShouldCreateGlobalEvent(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            var requestObject = EventFixtures.SomeEventRequest();
            var json = JsonConvert.SerializeObject(requestObject);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<EventResponse>(responseContent);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(responseObject.Title, requestObject.Title);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
}
