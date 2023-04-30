
using AdamTibi.OpenWeather;
//using OpenWeather;

namespace Uqs.Weather.Stubs;

public class ClientStub : IClient
{
    // private readonly double GREENWICH_LAT = 51.476852;
    // private readonly double GREENWICH_LON = -0.000500;
    private readonly DateTime _now;
    private readonly IEnumerable<double> _sevenDaysTemps;
    public Units? LastUnitSpy { get; set; }

    public ClientStub(DateTime now,
                        IEnumerable<double> sevenDaysTemp
                        //string apiKey,
                        //OpenWeather.Unit unit
                        ) //: base(apiKey, unit)
    {
        _now = now;
        _sevenDaysTemps = sevenDaysTemp;
    }

    /*
    public async Task<WeatherForecast[]> OneCallAsync(
                                decimal latitude,
                                decimal longitude
                                )
    {
        const int DAYS = 7;

        WeatherForecast[] wfs = new WeatherForecast[DAYS];


        for (var i = 0; i < DAYS; i++)
        {
            var newDate = _now.AddDays(i + 1);
            wfs[i] = new WeatherForecast()
            {
                Date = newDate,
                TemperatureC = _sevenDaysTemps.ElementAt(i),  //(int)Math.Round(res.Main.Temperature),
                Summary = "new weather"

            };

        }


        return await Task.FromResult<WeatherForecast[]>(wfs);
    } */

    public Task<OneCallResponse> OneCallAsync(decimal latitude,
                                            decimal longitude,
                                            IEnumerable<Excludes> excludes,
                                            Units unit)
    {
        LastUnitSpy = unit;
        const int DAYS = 7;
        OneCallResponse res = new OneCallResponse();
        res.Daily = new Daily[DAYS];
        for (int i = 0; i < DAYS; i++)
        {
            res.Daily[i] = new Daily();
            res.Daily[i].Dt = _now.AddDays(i);
            res.Daily[i].Temp = new Temp();
            res.Daily[i].Temp.Day = _sevenDaysTemps.ElementAt(i);
        }
        return Task.FromResult(res);
    }
}