using Microsoft.AspNetCore.Mvc;
using OpenWeather;

namespace Uqs.Weather.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class OpenWeatherForecastController : ControllerBase
{
    private readonly int FORECAST_DAYS = 5;
    private readonly double GREENWICH_LAT = 51.476852;
    private readonly double GREENWICH_LON = -0.000500;
    private readonly INowWrapper _nowWrapper;
    private readonly IRandomWrapper _randomWrapper;


    private readonly OpenWeatherClient? _openWeatherClient;



    private readonly ILogger<OpenWeatherForecastController> _logger;

    public OpenWeatherForecastController(ILogger<OpenWeatherForecastController> logger,
                                    IConfiguration configuration,
                                    IRandomWrapper randomWrapper,
                                    INowWrapper nowWrapper,
                                    OpenWeatherClient client)
    {
        this._logger = logger;
        this._randomWrapper = randomWrapper;
        this._nowWrapper = nowWrapper;
        this._openWeatherClient = client;
    }

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private string MapFeelToTemp(int temperatureC)
    {
        if (temperatureC <= 0) return Summaries.First();

        int summariesIndex = (temperatureC / 5) + 1;
        if (summariesIndex >= Summaries.Length)
            return Summaries.Last();
        return Summaries[summariesIndex];
    }


    [HttpGet(Name = "ConvertCToF2")]
    public double ConvertCToF2(double c,
                               [FromServices] ILogger<OpenWeatherForecastController> logger)
    {
        double f = c * (9d / 5d) + 32;
        logger.LogInformation("conversion requested");
        return f;
    }


    [HttpGet(Name = "GetReal_OpenWeather")]
    public async Task<IEnumerable<WeatherForecast>> GetReal_OpenWeather()
    {

        WeatherForecast[] wfs = new WeatherForecast[FORECAST_DAYS];


        if (this._openWeatherClient != null)
        {

            for (var i = 0; i < FORECAST_DAYS; i++)
            {
                var wf = wfs[i] = new WeatherForecast();

                // dependency unit
                WeatherData res = await this._openWeatherClient.GetWeatherAsync
                                ( GREENWICH_LAT,
                                  GREENWICH_LON
                                );

                res.DateTime = _nowWrapper.Now.AddDays(i + 1);
                wf.Date = res.DateTime.DateTime;

                wf.TemperatureC = (int)Math.Round(res.Main.Temperature);
                wf.Summary = MapFeelToTemp(wf.TemperatureC);
            }
        }

        Console.WriteLine(wfs);
        return wfs;
    }
}