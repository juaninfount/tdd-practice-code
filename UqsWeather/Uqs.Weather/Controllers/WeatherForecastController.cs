using AdamTibi.OpenWeather;
using Microsoft.AspNetCore.Mvc;

namespace Uqs.Weather.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class WeatherForecastController : ControllerBase
{
    private readonly int FORECAST_DAYS = 5;
    // private readonly double GREENWICH_LAT = 51.476852;
    // private readonly double GREENWICH_LON = -0.000500;
    private readonly INowWrapper _nowWrapper;
    private readonly IRandomWrapper _randomWrapper;
    private readonly IClient? _client;


    private readonly ILogger<WeatherForecastController> _logger;

  
    public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                    IClient client,
                                    IRandomWrapper randomWrapper,
                                    INowWrapper nowWrapper
                                    )
    {
        this._logger = logger;
        this._randomWrapper = randomWrapper;
        this._nowWrapper = nowWrapper;
        this._client = client;
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

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            //Date = DateTime.Now.AddDays(index),
            Date = this._nowWrapper.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet(Name = "GetRandom")]
    public IEnumerable<WeatherForecast> GetRandom()
    {
        WeatherForecast[] wfs = new WeatherForecast[FORECAST_DAYS];
        for (int i = 0; i < wfs.Length; i++)
        {
            var wf = wfs[i] = new WeatherForecast();
            wf.Date = this._nowWrapper.Now.AddDays(i + 1);
            //wf.Date = DateTime.Now.AddDays(i + 1);
            //wf.TemperatureC = Random.Shared.Next(-20, 55);
            wf.TemperatureC = this._randomWrapper.Next(-20, 55);
            wf.Summary = MapFeelToTemp(wf.TemperatureC);
        }

        return wfs;
    }

    [HttpGet(Name = "ConvertCToF")]
    public double ConvertCToF(double c,
                             [FromServices] ILogger<WeatherForecastController> logger)
    {
        double f = c * (9d / 5d) + 32;
        logger.LogInformation("conversion requested");
        return f;
    }

 
    [HttpGet("GetRealWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> GetReal()
    {
        const decimal GREENWICH_LAT = 51.4810m;
        const decimal GREENWICH_LON = 0.0052m;
        WeatherForecast[] wfs = new WeatherForecast[FORECAST_DAYS];

        if (_client != null)
        {
        
            for (int i = 0; i < wfs.Length; i++)
            {
                OneCallResponse res = await _client.OneCallAsync
                                        (GREENWICH_LAT, GREENWICH_LON, new[] {
                                            Excludes.Current, 
                                            Excludes.Minutely,
                                            Excludes.Hourly, 
                                            Excludes.Alerts }, 
                                            Units.Metric);

                var wf = wfs[i] = new WeatherForecast();
                wf.Date = res.Daily[i + 1].Dt;
                double forecastedTemp = res.Daily[i + 1].Temp.Day;
                wf.TemperatureC = (int)Math.Round(forecastedTemp);
                wf.Summary = MapFeelToTemp(wf.TemperatureC);
            }
        }
        return wfs;
    }
}
