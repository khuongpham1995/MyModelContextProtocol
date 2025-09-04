using System.Collections.Immutable;
using FluentValidation;
using MediatR;

namespace McpServer.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any()) return await next(cancellationToken);
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToImmutableArray();

        if (failures.Length != 0)
            throw new ValidationException(string.Join(Environment.NewLine, failures.Select(f => f.ErrorMessage)));

        return await next(cancellationToken);
    }
}