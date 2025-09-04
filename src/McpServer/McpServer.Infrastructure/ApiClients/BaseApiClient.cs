using System.Net.Http.Json;

namespace McpServer.Infrastructure.ApiClients;

public abstract class BaseApiClient<T>(HttpClient httpClient) where T : class
{
    protected readonly HttpClient _httpClient = httpClient;
    protected abstract string GeApiAction();

    public async Task<List<T>> GetAsync(CancellationToken ct)
    {
        var data = await _httpClient.GetFromJsonAsync<List<T>>(GeApiAction(), ct);
        return data ?? [];
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct)
    {
        var data = await _httpClient.GetFromJsonAsync<T>($"{GeApiAction()}/{id}", ct);
        return data;
    }

    public async Task<T> CreateAsync(T newEntity, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync(GeApiAction(), newEntity, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(ct)
               ?? throw new HttpRequestException("Failed to deserialize created entity.");
    }

    public async Task<T> UpdateAsync(int id, T updatedEntity, CancellationToken ct)
    {
        var response = await _httpClient.PutAsJsonAsync($"{GeApiAction()}/{id}", updatedEntity, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>(ct)
               ?? throw new HttpRequestException($"Failed to deserialize updated entity with ID '{id}'.");
    }

    public async Task DeleteAsync(int id, CancellationToken ct)
    {
        var response = await _httpClient.DeleteAsync($"{GeApiAction()}/{id}", ct);
        response.EnsureSuccessStatusCode();
    }
}