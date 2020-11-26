using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MeetingPlanner.Data;
using MeetingPlanner.Dto;
using MeetingPlanner.Others.Exceptions;
using MeetingPlanner.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace MeetingPlanner.Tests
{
    public class EventControllerTests : IntegrationTestBase
    {
        public EventControllerTests(CustomWebApplicationFactory<FakeStartup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("/api/events/global?date=2020-11-03")]
        public async Task GlobalEventsGetEndpoint_ShouldReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = GetFactory().CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            using var scope = GetFactory().Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(0, context.Events.Count());
        }

        [Theory]
        [InlineData("/api/events/global?date=2020-11-03")]
        public async Task GlobalEventsGetEndpoint_ShouldReturnBothEventsFromDatabase(string url)
        {
            // Arrange
            var client = GetFactory().CreateClient();

            using var scope = GetFactory().Server.Host.Services.CreateScope();
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
            var client = GetFactory().CreateClient();

            using var scope = GetFactory().Server.Host.Services.CreateScope();
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
            var client = GetFactory().CreateClient();
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

            using var scope = GetFactory().Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(1, context.Events.Count());
        }

        [Theory]
        [InlineData("/api/events/global")]
        public async Task GlobalEventsGetEndpoint_ShouldReturnExceptionDueToNoDateQueryParam(string url)
        {
            // Arrange
            var client = GetFactory().CreateClient();

            // Act
            var response = await client.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<ErrorDetails>(responseContent);

            // Assert

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal(500, responseBody.StatusCode);
            Assert.Equal(
                "Pobranie spotkań jest możliwe tylko w przypadku podania właściwego formatu daty w parametrze żądania!",
                responseBody.Message);
        }

        [Theory]
        [InlineData("/api/events?date=2020-11-03")]
        public async Task PrivateEventsGetEndpoint_ShouldReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = GetFactory(true).CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

            using var scope = GetFactory().Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(0, context.Events.Count());
        }

        [Theory]
        [InlineData("/api/events")]
        public async Task PrivateEventsPostEndpoint_ShouldCreatePersonalEvent(string url)
        {
            // Arrange
            var client = GetFactory(false, true).CreateClient();
            var requestObject = EventFixtures.SomeEventRequest().Global(false).Build();
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
            Assert.False(responseObject.Global);
            Assert.Equal(responseObject.Title, requestObject.Title);
            Assert.Empty(responseObject.Notifications);

            using var scope = GetFactory().Server.Host.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            Assert.Equal(1, context.Events.Count());
        }
    }
}