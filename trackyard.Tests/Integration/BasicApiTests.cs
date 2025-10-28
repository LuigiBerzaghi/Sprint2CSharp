using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Trackyard.Tests.Support;
using Xunit;

namespace Trackyard.Tests.Integration;

public class BasicApiTests : IClassFixture<TrackyardFactory>
{
    private readonly HttpClient _client;
    public BasicApiTests(TrackyardFactory factory)
    {
        _client = factory.CreateClient();
        _client.DefaultRequestHeaders.Add("X-API-KEY", "test-key");
    }

    [Fact]
    public async Task HealthEndpoint_Returns200()
    {
        var res = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task GetPatios_V1_Returns200()
    {
        var res = await _client.GetAsync("/api/v1/patios");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task MlPredict_ReturnsResult()
    {
        var payload = new { ano = DateTime.UtcNow.Year, quilometragem = 10000 };
        var json = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var res = await _client.PostAsync("/api/v1/ml/predict-risk", json);
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
        var text = await res.Content.ReadAsStringAsync();
        Assert.Contains("predicted", text);
    }
}
