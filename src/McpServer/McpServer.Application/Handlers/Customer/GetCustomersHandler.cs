using McpServer.Domain.Interfaces;
using MediatR;

namespace McpServer.Application.Handlers.Customer;

public record GetCustomersQuery : IRequest<IReadOnlyCollection<Domain.Entities.Customer>>;

public class GetCustomersHandler(ICustomerApiClient customerApiClient)
    : IRequestHandler<GetCustomersQuery, IReadOnlyCollection<Domain.Entities.Customer>>
{
    public async Task<IReadOnlyCollection<Domain.Entities.Customer>> Handle(GetCustomersQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await customerApiClient.GetAsync(cancellationToken);
        return customers;
    }
}