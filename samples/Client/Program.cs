using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Contract.Services.WeatherForecast;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var baseApiUrl = "https://localhost:7258";

builder.Services
    .AddRefitClient<IWeatherForecastService>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseApiUrl));

await builder.Build().RunAsync();