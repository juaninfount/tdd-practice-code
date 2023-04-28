using Xunit;
using Uqs.Weather;
using Uqs.Weather.Controllers;
using Microsoft.Extensions.Logging.Abstractions;

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
        var controller = new Uqs.Weather.Controllers.WeatherForecastController(logger, null!, null!, null!, null!);

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
        var logger = NullLogger<WeatherForecastController>.Instance;
        var controller = new WeatherForecastController(logger, null!, null!, null!, null!);

        // act
        double actual = controller.ConvertCToF(cGrades, logger);

        // Assert
        Assert.Equal(fGrades, actual, 1);
    }
}