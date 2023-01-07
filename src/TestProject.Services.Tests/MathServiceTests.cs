using FluentAssertions;
using TestProject.Services.Math;

namespace TestProject.Services.Tests
{
  public class MathServiceTests
  {
    protected readonly IMathService _sut;

    public MathServiceTests()
    {
      _sut = new MathService();
    }

    public class WhenAddIsCalled : MathServiceTests
    {
      [Fact]
      public void ItShould_AddNumbersCorrectly_WhenGivenNumbers()
      {
        var expectedResult = 10;

        var result = _sut.Add(5, 5);

        result.Should().Be(expectedResult);
      }
    }

    public class WhenMinusIsCalled : MathServiceTests
    {
      [Fact]
      public void ItShould_MinusNumbersCorrectly_WhenGivenNumbers()
      {
        var expectedResult = -5;

        var result = _sut.Minus(10, 15);

        result.Should().Be(expectedResult);
      }
    }

    public class WhenMultiplyIsCalled : MathServiceTests
    {
      [Fact]
      public void ItShould_MultiplyNumbersCorrectly_WhenGivenNumbers()
      {
        var expectedResult = 150;

        var result = _sut.Multiply(10, 15);

        result.Should().Be(expectedResult);
      }
    }

    public class WhenDivideIsCalled : MathServiceTests
    {
      [Fact]
      public void ItShould_DivideNumbersCorrectly_WhenGivenNumbers()
      {
        var expectedResult = 150;

        var result = _sut.Divide(1500, 10);

        result.Should().Be(expectedResult);
      }

      [Fact]
      public void ItShould_ThrowException_WhenDivisionByZero()
      {
        var result = _sut.Divide(10, 0);

        double.IsInfinity(result).Should().BeTrue();
      }
    }
  }
}
