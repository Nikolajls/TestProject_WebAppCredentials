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
      return Task.FromResult(request.Number1 + request.Number2);
    }
  }
}
