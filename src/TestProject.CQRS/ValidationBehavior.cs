﻿using FluentValidation;
using MediatR;

namespace TestProject.CQRS;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{

  private readonly IEnumerable<IValidator<TRequest>> _validators;
  public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

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
      throw new ValidationException(listOfFailures);
    }
    return await next();
  }
}
