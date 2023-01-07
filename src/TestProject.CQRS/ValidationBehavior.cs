using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TestProject.CQRS;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{

  private readonly IEnumerable<IValidator<TRequest>> _validators;
  private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
  public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
  {
    _validators = validators;
    _logger = logger;
  }

  public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
  {
    if (!_validators.Any())
    {
      return await next();
    }

    var context = new ValidationContext<TRequest>(request);
    var listOfFailures = _validators
        .Select(x => x.Validate(context))
        .SelectMany(x => x.Errors)
        .Where(x => x != null)
        .ToList();
    //.GroupBy(
    //    x => x.PropertyName,
    //    x => x.ErrorMessage,
    //    (propertyName, errorMessages) => new
    //    {
    //      Key = propertyName,
    //      Values = errorMessages.Distinct().ToArray()
    //    })
    //.ToDictionary(x => x.Key, x => x.Values);

    if (listOfFailures.Count > 0)
    {
      _logger.LogError("ValidationPipeline failed for {Type}", typeof(TRequest).Name);
      throw new ValidationException(listOfFailures);
    }

    return await next();
  }
}

