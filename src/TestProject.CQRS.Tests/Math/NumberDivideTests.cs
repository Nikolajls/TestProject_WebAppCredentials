using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestProject.CQRS.Math;
using TestProject.Services.Math;
using static TestProject.CQRS.Math.NumberDivide;

namespace TestProject.CQRS.Tests.Math;

public class NumberDivideTests
{
  private readonly Mock<ILogger<NumberDivideHandler>> loggerMock = new();
  private readonly Mock<IMathService> _serviceMock = new();
  private readonly NumberDivideHandler _sut;

  public NumberDivideTests()
  {
    _sut = new NumberDivideHandler(loggerMock.Object, _serviceMock.Object);
  }

  [Theory]
  [InlineData(100, 5, 20)]
  [InlineData(10, 20, 0.5)]
  [InlineData(1000, -2, -500)]
  public async Task ItShould_AddNumbersCorrectly(double number, double divideBy, double expectedResult)
  {
    _serviceMock.Setup(c => c.Divide(number, divideBy)).Returns(expectedResult);

    var query = new NumberDivide.Query
    {
      Number = number,
      DivideBy = divideBy,
    };

    var result = await _sut.Handle(query, default);

    result.Should().Be(expectedResult);
  }
}
