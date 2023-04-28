using Microsoft.AspNetCore.Mvc;
//using AdamTibi.OpenWeather;
using OpenWeather;


namespace Uqs.Weather.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
public class WeatherForecastController : ControllerBase
{   
    private readonly int FORECAST_DAYS = 5;
    private readonly double GREENWICH_LAT = 51.476852;
    private readonly double GREENWICH_LON = -0.000500;
    private readonly INowWrapper _nowWrapper;
    private readonly IRandomWrapper _randomWrapper;


    private readonly OpenWeatherClient _IClient;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                    IConfiguration configuration,
                                    IRandomWrapper randomWrapper,
                                    INowWrapper nowWrapper,
                                    OpenWeatherClient client)
    {
        this._logger = logger;
        this._randomWrapper = randomWrapper;
        this._nowWrapper = nowWrapper;
        this._IClient = client;
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

    [HttpGet(Name = "GetReal")]
    public async Task<IEnumerable<WeatherForecast>> GetReal()
    {
        //string apiKey = _configuration["OpenWeather:Key"];
        //HttpClient httpClient = new HttpClient();
        //_client = new Client(apiKey, httpClient);
        //Client openWeatherClient = new Client(apiKey, httpClient);
                          
        /*
        Console.WriteLine(" Instancia de openWeather => " +
                          $" Nubes: ${res.Clouds.Cloudiness} " +
                          $" Clima: ${res.Weather[0].Description} " +  
                          $" Temp.: ${res.Main.Temperature.ToString()} " );   */
        WeatherForecast[] wfs = new WeatherForecast[FORECAST_DAYS];

         
        for (var i = 0; i < FORECAST_DAYS; i++)  
        {
            var wf = wfs[i] = new WeatherForecast();

            WeatherData res = await this._IClient.GetWeatherAsync
                            ( GREENWICH_LAT,
                              GREENWICH_LON
                            );

            res.DateTime = _nowWrapper.Now.AddDays(i+1);              
            wf.Date = res.DateTime.DateTime;    

            wf.TemperatureC = (int)Math.Round(res.Main.Temperature);
            wf.Summary = MapFeelToTemp(wf.TemperatureC);
        }
    

        return wfs;
    }
}
