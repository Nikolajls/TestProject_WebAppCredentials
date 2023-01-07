using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestProject.CQRS.Math;
using TestProject.Services.Math;
using static TestProject.CQRS.Math.NumberMinus;

namespace TestProject.CQRS.Tests.Math;

public class NumberMinusTests
{
  private readonly Mock<ILogger<NumberMinusHandler>> loggerMock = new();
  private readonly Mock<IMathService> _serviceMock = new();
  private readonly NumberMinusHandler _sut;

  public NumberMinusTests()
  {
    _sut = new NumberMinusHandler(loggerMock.Object, _serviceMock.Object);
  }

  [Theory]
  [InlineData(100, 5, 95)]
  [InlineData(10, 20, -10)]
  [InlineData(1000, -2, 1002)]
  public async Task ItShould_MinusNumbersCorrectly(double number, double minusWith, double expectedResult)
  {
    _serviceMock.Setup(c => c.Minus(number, minusWith)).Returns(expectedResult);

    var query = new NumberMinus.Query
    {
      Number = number,
      MinusWith = minusWith,
    };

    var result = await _sut.Handle(query, default);

    result.Should().Be(expectedResult);
  }
}
