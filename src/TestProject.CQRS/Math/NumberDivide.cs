using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TestProject.CQRS.Math;

public class NumberDivide
{
  public class Query : IRequest<double>
  {
    public double Number { get; set; }
    public double DivideBy { get; set; }
  }


  public class Validator : AbstractValidator<Query>
  {
    public Validator()
    {
      RuleFor(c => c.DivideBy).NotEqual(0);
    }
  }

  public class NumberDivideHandler : IRequestHandler<Query, double>
  {
    private readonly ILogger<NumberDivideHandler> _logger;

    public NumberDivideHandler(ILogger<NumberDivideHandler> logger)
    {
      _logger = logger;
    }

    public Task<double> Handle(Query request, CancellationToken cancellationToken)
    {
      var result = request.Number / request.DivideBy;

      _logger.LogInformation("Diving numbers {Number1}/{DivideBy} resulted in: {Result}", request.Number, request.DivideBy, result);

      return Task.FromResult(result);
    }
  }
}
