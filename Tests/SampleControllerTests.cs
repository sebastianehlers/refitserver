using System.Net.Http.Json;
using Contract.Services.WeatherForecast;
using Contract.Services.WeatherForecast.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Refit;

namespace Tests;

public class SampleControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly IWeatherForecastService _forecastService;

    public SampleControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _forecastService = RestService.For<IWeatherForecastService>(_client);
    }
    
    [Fact]
    public async Task CanGetWithRouteParameter()
    {
        var resp = await _forecastService.GetSingleForecastById(22);
        Assert.IsType<WeatherForecastDto>(resp);
        Assert.StartsWith("22", resp.Summary);
    }
    
    [Fact]
    public async Task CannotGetWithoutRequiredRouteParameter()
    {
        var resp = await Assert.ThrowsAsync<HttpRequestException>(() => _client.GetFromJsonAsync<WeatherForecastDto>("/api/v1/weatherforecast/single/"));
    }
    
    [Fact]
    public async Task CanGetWithRouteAndQueryParameters()
    {
        var resp = await _forecastService.GetForecastsForCountry(22, "search query here");
        Assert.IsAssignableFrom<IEnumerable<WeatherForecastDto>>(resp);
        Assert.All(resp, dto => Assert.StartsWith("search query here", dto.Summary));
    }

    [Fact]
    public async Task CanPostBody()
    {
        var dto = new WeatherForecastDto
        {
            Date = DateOnly.FromDateTime(DateTime.Now),
            Summary = Guid.NewGuid().ToString(),
            TemperatureC = new Random().Next()
        };
        var resp = await _forecastService.CreateForecast(dto);
        
        Assert.Equal(dto.Date, resp.Date);
        Assert.Equal(dto.Summary, resp.Summary);
        Assert.Equal(dto.TemperatureC, resp.TemperatureC);
    }

    [Fact]
    public async Task AttributeOnMethodIsKept()
    {
        await Assert.ThrowsAsync<ApiException>(() => _forecastService.DeleteCountryById(22));
    }
}