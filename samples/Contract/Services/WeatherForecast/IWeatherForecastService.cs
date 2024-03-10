using Contract.Services.WeatherForecast.Models;
using Refit;

namespace Contract.Services.WeatherForecast;

public interface IWeatherForecastService
{
    [Get("/api/v1/weatherforecasts/{countryId}")]
    public Task<IEnumerable<WeatherForecastDto>> GetForecastsForCountry(int countryId, string searchQuery);
    
    [Delete("/api/v1/weatherforecasts/{countryId}")]
    public Task DeleteCountryById(int countryId);

    [Get("/api/v1/weatherforecast/single/{forecastId}")]
    public Task<WeatherForecastDto> GetSingleForecastById(int forecastId);

    [Post("/api/v1/weatherforecast")]
    public Task CreateForecast([Body]WeatherForecastDto newForecast);
}