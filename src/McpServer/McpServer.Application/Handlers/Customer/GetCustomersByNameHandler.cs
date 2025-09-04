using FluentValidation;
using McpServer.Domain.Interfaces;
using MediatR;

namespace McpServer.Application.Handlers.Customer;

public record GetCustomersByNameQuery(string? Name) : IRequest<IReadOnlyCollection<Domain.Entities.Customer>>;

public class GetCustomersByNameHandler(ICustomerApiClient customerApiClient)
    : IRequestHandler<GetCustomersByNameQuery, IReadOnlyCollection<Domain.Entities.Customer>>
{
    public async Task<IReadOnlyCollection<Domain.Entities.Customer>> Handle(GetCustomersByNameQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await customerApiClient.GetByNameAsync(request.Name!, cancellationToken);
        return customers;
    }
}

public class GetCustomersByNameQueryValidator : AbstractValidator<GetCustomersByNameQuery>
{
    public GetCustomersByNameQueryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.");
    }
}