using McpServer.Domain.Interfaces;
using Microsoft.KernelMemory;

namespace McpServer.Infrastructure.Services;

public class KernelMemoryService(IKernelMemory kernelMemory) : IKernelMemoryService
{
    public async Task<string> AskAsync(string question, CancellationToken cancellationToken = default)
    {
        var result = await kernelMemory.AskAsync(question, cancellationToken: cancellationToken);
        return result.Result;
    }
    
    public async Task<string> UploadDocumentAsync(string filePath, string documentId, CancellationToken cancellationToken = default)
    {
        var result = await kernelMemory.ImportDocumentAsync(filePath, documentId, cancellationToken: cancellationToken);
        return result;
    }
    
    public async Task<bool> IsDocumentReadyAsync(string documentId, CancellationToken cancellationToken = default)
    {
        var result = await kernelMemory.IsDocumentReadyAsync(documentId, cancellationToken: cancellationToken);
        return result;
    }
}