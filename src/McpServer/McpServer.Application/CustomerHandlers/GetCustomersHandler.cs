using McpServer.Domain.Entities;
using McpServer.Domain.Interfaces;
using MediatR;

namespace McpServer.Application.CustomerHandlers;

public record GetCustomersQuery() : IRequest<IReadOnlyCollection<Customer>>;

public class GetCustomersHandler(ICustomerApiClient customerApiClient) : IRequestHandler<GetCustomersQuery, IReadOnlyCollection<Customer>>
{
    public async Task<IReadOnlyCollection<Customer>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await customerApiClient.GetAsync(cancellationToken);
        return customers;
    }
}