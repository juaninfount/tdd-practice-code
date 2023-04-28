using System;
using Xunit;

namespace Uqs.Arithmetic.Tests.Unit;

// ClassNameTests
public class UnitTest1
{

    //public void MethodName_Condition1_Expectation1()
    [Fact]
    public void Test1()
    { 
       //Arrange: declare some variables and do some preparations.

       // Act: when we invoke the SUT

       // Assert: where we validate our assumption

    }

    [ Fact ]
    public void Divide_DivisibleIntegers_WholeNumber()
    {
        // Arrange
        int dividend = 10;
        int divisor = 5;
        decimal expectedQuotient = 2;

        // Act
        decimal actualQuotient = Division.Divide(dividend, divisor);

        // Assert
        Assert.Equal(actualQuotient, expectedQuotient);
    }

    [Fact]
    public void Divide_ZeroDivisor_DivideByZeroException()
    {
        int dividend = 10;
        int divisor = 0;

        Exception e = Record.Exception( () => Division.Divide(dividend,divisor));

        Assert.IsType<DivideByZeroException>(e);
    }
}