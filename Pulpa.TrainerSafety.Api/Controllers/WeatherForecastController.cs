using Microsoft.AspNetCore.Mvc;

namespace Pulpa.TrainerSafety.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<string> Get()
    {
        return Enumerable.Range(1, 5).Select(index => index.ToString())
        .ToArray();
    }
}
