namespace McpServer.Domain.Models;

public class ResponseModel<T>
{
    public string? Error { get; set; }
    public T? Data { get; set; }
    public bool IsSuccess {get; set;}

    public void Succeed(T? data = default)
    {
        Error = string.Empty;
        Data = data;
        IsSuccess = true;
    }

    public void Fail(string? error = default)
    {
        Error = error;
        Data = default;
        IsSuccess = false;
    }
}
