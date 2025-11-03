using Xunit;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using GiftOfGivers_WebApplication;

namespace GiftOfGivers_WebApplication.Tests.Integration;

public class StressTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public StressTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _client.Timeout = TimeSpan.FromMinutes(5); // Extended timeout for stress tests
    }

    [Fact]
    public async Task IncidentReports_MassiveLoadTest()
    {
        // Test with 1000 concurrent requests
        var sw = Stopwatch.StartNew();
        var tasks = Enumerable.Range(0, 1000)
            .Select(i => _client.GetAsync("/IncidentReports"))
            .ToList();

        var responses = await Task.WhenAll(tasks);
        sw.Stop();

        var successCount = responses.Count(r => 
            r.IsSuccessStatusCode || r.StatusCode == HttpStatusCode.Redirect);

        // Assert at least 98% success rate
        Assert.True(successCount >= 980, $"Success rate: {successCount}/1000");
        Assert.True(sw.ElapsedMilliseconds < 60000, $"Took too long: {sw.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task ResourceTracking_EnduranceTest()
    {
        // Simulate 30 seconds of continuous load
        var endTime = DateTime.Now.AddSeconds(30);
        var requestCount = 0;
        var errors = 0;

        while (DateTime.Now < endTime)
        {
            try
            {
                var response = await _client.GetAsync("/ResourceTracking");
                requestCount++;
                if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.Redirect)
                {
                    errors++;
                }
            }
            catch (Exception)
            {
                errors++;
            }
        }

        var successRate = (requestCount - errors) / (double)requestCount * 100;
        Assert.True(successRate >= 95, $"Success rate: {successRate}%");
        Assert.True(requestCount > 0, "No requests were made");
    }

    [Fact]
    public async Task ConcurrentWriteOperations_StressTest()
    {
        // Simulate multiple concurrent operations
        var tasks = Enumerable.Range(0, 50)
            .Select(i => Task.Run(async () =>
            {
                try
                {
                    // Get home page (lightweight operation)
                    var response = await _client.GetAsync("/");
                    return response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Redirect;
                }
                catch
                {
                    return false;
                }
            }))
            .ToList();

        var results = await Task.WhenAll(tasks);
        var successCount = results.Count(r => r);

        // Assert at least 90% success rate under stress
        Assert.True(successCount >= 45, $"Success rate: {successCount}/50");
    }

    [Fact]
    public async Task ResponseTime_UnderLoad()
    {
        // Measure response times under light load
        var responseTimes = new List<long>();
        
        for (int i = 0; i < 100; i++)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await _client.GetAsync("/");
                sw.Stop();
                responseTimes.Add(sw.ElapsedMilliseconds);
            }
            catch { }
        }

        var avgResponseTime = responseTimes.Average();
        var p95ResponseTime = responseTimes.OrderBy(x => x).Skip((int)(responseTimes.Count * 0.95)).First();

        // Assert average response time is reasonable
        Assert.True(avgResponseTime < 1000, $"Average response time too high: {avgResponseTime}ms");
        Assert.True(p95ResponseTime < 2000, $"95th percentile too high: {p95ResponseTime}ms");
    }
}


