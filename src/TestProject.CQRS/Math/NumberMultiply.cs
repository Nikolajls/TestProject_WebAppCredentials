using MediatR;
using Microsoft.Extensions.Logging;

namespace TestProject.CQRS.Math;

public class NumberMultiply
{
  public class Query : IRequest<double>
  {
    public double Number1 { get; set; }
    public double Number2 { get; set; }
  }


  public class NumberMultiplyHandler : IRequestHandler<Query, double>
  {
    private readonly ILogger<NumberMultiplyHandler> _logger;

    public NumberMultiplyHandler(ILogger<NumberMultiplyHandler> logger)
    {
      _logger = logger;
    }

    public Task<double> Handle(Query request, CancellationToken cancellationToken)
    {
      var result = request.Number1 * request.Number2;

      _logger.LogInformation("Multiplying numbers {Number}*{MinusWith} resulted in: {Result}", request.Number1, request.Number2, result);

      return Task.FromResult(result);
    }
  }
}
