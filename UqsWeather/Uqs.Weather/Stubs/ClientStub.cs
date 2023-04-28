
using OpenWeather;
using Uqs.Weather;

public class ClientStub : OpenWeatherClient
{
    private readonly double GREENWICH_LAT = 51.476852;
    private readonly double GREENWICH_LON = -0.000500;
    public ClientStub(string apiKey, OpenWeather.Unit unit) : base(apiKey, unit)
    {
    }

    public async Task<WeatherForecast[]> OneCallAsync(
                                decimal latitude,
                                decimal longitude
                                )
    {
        const int DAYS = 7;
        /*
        OneCallResponse res = new OneCallResponse();
        res.Daily = new Daily[DAYS];
        */
        WeatherData res = await this.GetWeatherAsync
                        ( GREENWICH_LAT,
                          GREENWICH_LON
                        );

        WeatherForecast[] wfs = new WeatherForecast[DAYS];

        if (res != null)
        {
            for (var i = 0; i < DAYS; i++)
            {
                res.DateTime = DateTime.Now.AddDays(i + 1);
                wfs[i] = new WeatherForecast()
                {
                    Date = res.DateTime.DateTime,
                    TemperatureC = (int)Math.Round(res.Main.Temperature),
                    Summary = res.Weather[0].Description
                };

            }
        }

        return await Task.FromResult<WeatherForecast[]>(wfs);
    }
}