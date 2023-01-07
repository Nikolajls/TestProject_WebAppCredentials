using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Services.Math
{
  public interface IMathService
  {
    double Add(double number1, double number2);
    double Multiply(double number1, double number2);
    double Divide(double number, double divideBy);
    double Minus(double number, double minusWith);
  }
}
