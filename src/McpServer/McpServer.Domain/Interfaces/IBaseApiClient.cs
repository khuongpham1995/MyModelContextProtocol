namespace McpServer.Domain.Interfaces;

public interface IBaseApiClient<T> where T : class
{
    Task<List<T>> GetAsync(CancellationToken ct);
    Task<T?> GetByIdAsync(int id, CancellationToken ct);
    Task<T> CreateAsync(T newEntity, CancellationToken ct);
    Task<T> UpdateAsync(int id, T updatedEntity, CancellationToken ct);
    Task DeleteAsync(int id, CancellationToken ct);
}