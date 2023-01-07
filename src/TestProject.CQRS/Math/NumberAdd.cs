using MediatR;
using Microsoft.Extensions.Logging;

namespace TestProject.CQRS.Math;

public class NumberAdd
{
  public class Query : IRequest<double>
  {
    public double Number1 { get; set; }
    public double Number2 { get; set; }
  }

  public class NumberAddHandler : IRequestHandler<Query, double>
  {
    private readonly ILogger<NumberAddHandler> _logger;

    public NumberAddHandler(ILogger<NumberAddHandler> logger)
    {
      _logger = logger;
    }

    public Task<double> Handle(Query request, CancellationToken cancellationToken)
    {
      var result = request.Number1 + request.Number2;
      _logger.LogInformation("Adding numbers {Number1} + {Number2} resulted in: {Result}", request.Number1, request.Number2, result);
      return Task.FromResult(result);
    }
  }
}
