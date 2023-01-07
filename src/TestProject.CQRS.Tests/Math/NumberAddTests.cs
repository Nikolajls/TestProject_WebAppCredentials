using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TestProject.CQRS.Math;
using TestProject.Services.Math;
using static TestProject.CQRS.Math.NumberAdd;

namespace TestProject.CQRS.Tests.Math;

public class NumberAddTests
{

  private readonly Mock<ILogger<NumberAddHandler>> loggerMock = new();
  private readonly Mock<IMathService> _serviceMock = new();
  private readonly NumberAddHandler _sut;

  public NumberAddTests()
  {
    _sut = new NumberAddHandler(loggerMock.Object, _serviceMock.Object);
  }

  [Theory]
  [InlineData(100, 200, 300)]
  [InlineData(-100, 200, 100)]
  [InlineData(-500, -500, -1000)]
  public async Task ItShould_AddNumbersCorrectly(double number1, double number2, double expectedResult)
  {
    _serviceMock.Setup(c => c.Add(number1, number2)).Returns(expectedResult);

    var query = new NumberAdd.Query
    {
      Number1 = number1,
      Number2 = number2,
    };


    var result = await _sut.Handle(query, default);

    result.Should().Be(expectedResult);
  }
}
