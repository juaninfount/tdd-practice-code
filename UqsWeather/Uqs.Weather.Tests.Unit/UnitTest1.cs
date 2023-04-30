using Xunit;
using Uqs.Weather.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Uqs.Weather.Tests.Unit;

public class UnitTest1
{
    [Fact]
    // Method_Condition_Expectation
    public void ConvertCToF_0Celsius_32Fahrenheit()
    {
        // arrange
        const double expected = 32d;
        var logger = NullLogger<WeatherForecastController>.Instance;
        var controller = new Uqs.Weather.Controllers.WeatherForecastController(logger, null!, null!, null!);

        // act
        double actual = controller.ConvertCToF(0, logger);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(-100, -148)]    // cada linea actua como una unica prueba unitaria 
    [InlineData(-10.1, 13.8)]
    [InlineData(10, 50)]
    // Method_Condition_Expectation
    public void ConvertCToF_Cel_CorrectFah(double cGrades, double fGrades)
    {
        // arrange
        var logger = NullLogger<OpenWeatherForecastController>.Instance;
        var controller = new OpenWeatherForecastController(logger, null!, null!, null!, null!);

        // act
        double actual = controller.ConvertCToF2(cGrades, logger);

        // Assert
        Assert.Equal(fGrades, actual, 1);
    }

    [Fact]
    public async Task GetReal_NotInterestedInTodayWeather_WFStartsFromNextDay()
    {
         // Arrange
        const double nextDayTemp = 3;
        const double day5Temp = 7;
        var today = new DateTime(2022, 1, 1);
        var realWeatherTemps = new[] {2, nextDayTemp, 4, 5, 6, day5Temp, 8};
        var clientStub = new Uqs.Weather.Stubs.ClientStub(today, realWeatherTemps);
        var controller = new WeatherForecastController(null!, clientStub, null!, null!);

        // Act
        IEnumerable<WeatherForecast> wfs = await controller.GetReal();

        // Assert
        Assert.Equal(3, wfs.First().TemperatureC);
    }

}