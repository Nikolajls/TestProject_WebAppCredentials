using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestProject.CQRS.Math;
using TestProject.Services.Math;
using static TestProject.CQRS.Math.NumberMultiply;

namespace TestProject.CQRS.Tests.Math;

public class NumberMultiplyTests
{
  private readonly Mock<ILogger<NumberMultiplyHandler>> loggerMock = new();
  private readonly Mock<IMathService> _serviceMock = new();
  private readonly NumberMultiplyHandler _sut;

  public NumberMultiplyTests()
  {
    _sut = new NumberMultiplyHandler(loggerMock.Object, _serviceMock.Object);
  }

  [Theory]
  [InlineData(100, 5, 500)]
  [InlineData(10, 20, 200)]
  public async Task ItShould_MultiplyNumbersCorrectly(double number1, double number2, double expectedResult)
  {
    _serviceMock.Setup(c => c.Multiply(number1, number2)).Returns(expectedResult);


    var query = new NumberMultiply.Query
    {
      Number1 = number1,
      Number2 = number2,
    };

    var result = await _sut.Handle(query, default);

    result.Should().Be(expectedResult);
  }
}
