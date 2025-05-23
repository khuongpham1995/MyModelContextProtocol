using FluentValidation;
using McpServer.Domain.Entities;
using McpServer.Domain.Interfaces;
using MediatR;

namespace McpServer.Application.CustomerHandlers;

public record GetCustomersByNameQuery(string? Name) : IRequest<IReadOnlyCollection<Customer>>;

public class GetCustomersByNameHandler(ICustomerApiClient customerApiClient) : IRequestHandler<GetCustomersByNameQuery, IReadOnlyCollection<Customer>>
{
    public async Task<IReadOnlyCollection<Customer>> Handle(GetCustomersByNameQuery request, CancellationToken cancellationToken)
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