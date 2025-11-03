using Xunit;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using GiftOfGivers_WebApplication;
using GiftOfGivers_WebApplication.Models;

namespace GiftOfGivers_WebApplication.Tests.Integration;

public class ApiLoadTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public ApiLoadTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task IncidentReports_Index_LoadTest()
    {
        // Arrange
        var tasks = new List<Task<HttpResponseMessage>>();

        // Act - Simulate 100 concurrent requests
        for (int i = 0; i < 100; i++)
        {
            tasks.Add(_client.GetAsync("/IncidentReports"));
        }

        var responses = await Task.WhenAll(tasks);

        // Assert
        Assert.All(responses, response => 
        {
            // Should either succeed or redirect to login
            Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Redirect);
        });
    }

    [Fact]
    public async Task ResourceTracking_Index_StressTest()
    {
        // Arrange
        var tasks = new List<Task<HttpResponseMessage>>();

        // Act - Simulate 500 concurrent requests
        for (int i = 0; i < 500; i++)
        {
            tasks.Add(_client.GetAsync("/ResourceTracking"));
        }

        var responses = await Task.WhenAll(tasks);
        var successCount = responses.Count(r => r.IsSuccessStatusCode || r.StatusCode == HttpStatusCode.Redirect);

        // Assert - At least 95% should succeed
        Assert.True(successCount >= 475, $"Success rate: {successCount}/500");
    }

    [Fact]
    public async Task Home_Index_PerformanceTest()
    {
        // Arrange
        var sw = System.Diagnostics.Stopwatch.StartNew();

        // Act - Simulate 50 concurrent requests
        var tasks = Enumerable.Range(0, 50)
            .Select(_ => _client.GetAsync("/"))
            .ToList();

        var responses = await Task.WhenAll(tasks);
        sw.Stop();

        // Assert
        Assert.All(responses, r => Assert.True(r.IsSuccessStatusCode || r.StatusCode == HttpStatusCode.Redirect));
        Assert.True(sw.ElapsedMilliseconds < 5000, $"Response time too slow: {sw.ElapsedMilliseconds}ms");
    }
}
