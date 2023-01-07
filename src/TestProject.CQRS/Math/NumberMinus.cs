using MediatR;
using Microsoft.Extensions.Logging;

namespace TestProject.CQRS.Math;

public class NumberMinus
{
  public class Query : IRequest<double>
  {
    public double Number { get; set; }
    public double MinusWith { get; set; }
  }


  public class NumberMinusHandler : IRequestHandler<Query, double>
  {
    private readonly ILogger<NumberMinusHandler> _logger;

    public NumberMinusHandler(ILogger<NumberMinusHandler> logger)
    {
      _logger = logger;
    }

    public Task<double> Handle(Query request, CancellationToken cancellationToken)
    {
      return Task.FromResult(request.Number - request.MinusWith);
    }
  }
}
