using System.Net.Http.Json;
using System.Text;
using McpServer.Domain.Entities;
using McpServer.Domain.Interfaces;

namespace McpServer.Infrastructure.ApiClients;

public class CustomerApiClient(
    HttpClient httpClient) : BaseApiClient<Customer>(httpClient), ICustomerApiClient
{
    public async Task<IReadOnlyCollection<Customer>> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        var url = new StringBuilder(GeApiAction());
        url.Append($"?name={name}");
        var data = await _httpClient.GetFromJsonAsync<List<Customer>>(url.ToString(), cancellationToken);
        return data ?? [];
    }

    protected override string GeApiAction() => "/api/v1/customer";
}
