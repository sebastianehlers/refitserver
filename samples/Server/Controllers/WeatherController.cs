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
    private IEnumerable<WeatherForecastDto> List(string summary)
    {
        return Enumerable.Range(0, 10)
            .Select(i => new WeatherForecastDto
            {
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(i)),
                Summary = summary + " " + Guid.NewGuid().ToString(),
                TemperatureC = (i * 5) % 20
            });
    }
    
    [RequireHttps]
    public async Task<IEnumerable<WeatherForecastDto>> GetForecastsForCountry(int countryId, [NotNull][NotNullWhen(true)]string searchQuery)
    {
        await Task.Delay(1);

        return List(searchQuery);
    }

    [Authorize("blabla", Roles = "teste")]
    public async Task DeleteCountryById(int countryId)
    {
        await Task.Delay(2);
    }

    public async Task<WeatherForecastDto> GetSingleForecastById([Required]int forecastId)
    {
        return List(forecastId.ToString()).First();
    }

    public async Task<WeatherForecastDto> CreateForecast(WeatherForecastDto newForecast)
    {
        await Task.Delay(1);
        return newForecast;
    }
}