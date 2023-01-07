using MediatR;
using Microsoft.Extensions.Logging;
using TestProject.Services.Math;

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
    private readonly IMathService _mathService;

    public NumberMinusHandler(ILogger<NumberMinusHandler> logger, IMathService mathService)
    {
      _logger = logger;
      _mathService = mathService;
    }

    public Task<double> Handle(Query request, CancellationToken cancellationToken)
    {
      var result = _mathService.Minus(request.Number, request.MinusWith);

      _logger.LogInformation("Minusing numbers {Number}/{MinusWith} resulted in: {Result}", request.Number, request.MinusWith, result);

      return Task.FromResult(result);
    }
  }
}
