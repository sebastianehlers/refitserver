using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Contract.Services.WeatherForecast;
using Contract.Services.WeatherForecast.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RefitServer.Attributes;

namespace Server.Controllers;

[GenerateRefitController]
public partial class WeatherController : Controller, IWeatherForecastService
{
    private IEnumerable<WeatherForecastDto> List()
    {
        return Enumerable.Range(0, 10)
            .Select(i => new WeatherForecastDto
            {
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(i)),
                Summary = Guid.NewGuid().ToString(),
                TemperatureC = (i * 5) % 20
            });
    }
    
    [RequireHttps]
    public async Task<IEnumerable<WeatherForecastDto>> GetForecastsForCountry(int countryId, [NotNull][NotNullWhen(true)]string searchQuery)
    {
        await Task.Delay(500);

        return List();
    }

    [Authorize("blabla", Roles = "teste")]
    public async Task DeleteCountryById(int countryId)
    {
        await Task.Delay(300);
    }

    public async Task<WeatherForecastDto> GetSingleForecastById([Required]int forecastId)
    {
        return List().First();
    }

    public Task CreateForecast(WeatherForecastDto newForecast)
    {
        return Task.Delay(500);
    }
}