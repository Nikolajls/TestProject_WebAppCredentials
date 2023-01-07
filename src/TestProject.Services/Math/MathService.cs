namespace TestProject.Services.Math
{
  public class MathService : IMathService
  {
    public double Add(double number1, double number2) => number1 + number2;

    public double Divide(double number, double divideBy) => number / divideBy;

    public double Minus(double number, double minusWith) => number - minusWith;

    public double Multiply(double number1, double number2) => number1 * number2;
  }
}
