using McpServer.Domain.Entities;

namespace McpServer.Domain.Interfaces;

public interface ICustomerApiClient : IBaseApiClient<Customer>
{
    Task<IReadOnlyCollection<Customer>> GetByNameAsync(string name, CancellationToken cancellationToken);
}
