using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using McpServer.Infrastructure.Services;
using Microsoft.KernelMemory;

namespace McpServer.Tests.Infrastructure.Services;

public class KernelMemoryServiceTests
{
    [Fact]
    public async Task AskAsync_ReturnsKernelAnswerResult()
    {
        const string question = "What is the summary?";
        const string expected = "This is the answer.";
        string? capturedQuestion = null;
        var invocationCount = 0;

        var kernel = KernelMemoryDispatchProxy.Create((method, args) =>
        {
            if (method.Name == nameof(IKernelMemory.AskAsync))
            {
                invocationCount++;
                capturedQuestion = args[0] as string;
                return Task.FromResult(new MemoryAnswer { Result = expected });
            }

            throw new NotImplementedException(method.Name);
        });

        var service = new KernelMemoryService(kernel);

        var result = await service.AskAsync(question);

        result.Should().Be(expected);
        capturedQuestion.Should().Be(question);
        invocationCount.Should().Be(1);
    }

    [Fact]
    public async Task UploadDocumentAsync_ReturnsDocumentIdFromKernel()
    {
        const string filePath = "path/to/document.txt";
        const string documentId = "document-id";
        string? capturedFilePath = null;
        string? capturedDocumentId = null;

        var kernel = KernelMemoryDispatchProxy.Create((method, args) =>
        {
            if (method.Name == nameof(IKernelMemory.ImportDocumentAsync) && args.Length >= 2 && args[0] is string)
            {
                capturedFilePath = args[0] as string;
                capturedDocumentId = args[1] as string;
                return Task.FromResult(documentId);
            }

            throw new NotImplementedException(method.Name);
        });

        var service = new KernelMemoryService(kernel);

        var result = await service.UploadDocumentAsync(filePath, documentId);

        result.Should().Be(documentId);
        capturedFilePath.Should().Be(filePath);
        capturedDocumentId.Should().Be(documentId);
    }

    [Fact]
    public async Task IsDocumentReadyAsync_ReturnsUnderlyingValue()
    {
        const string documentId = "doc";
        string? capturedDocumentId = null;

        var kernel = KernelMemoryDispatchProxy.Create((method, args) =>
        {
            if (method.Name == nameof(IKernelMemory.IsDocumentReadyAsync))
            {
                capturedDocumentId = args[0] as string;
                return Task.FromResult(true);
            }

            throw new NotImplementedException(method.Name);
        });

        var service = new KernelMemoryService(kernel);

        var result = await service.IsDocumentReadyAsync(documentId);

        result.Should().BeTrue();
        capturedDocumentId.Should().Be(documentId);
    }

    private class KernelMemoryDispatchProxy : DispatchProxy
    {
        public delegate object? InvocationHandler(MethodInfo method, object?[] args);

        public InvocationHandler? Handler { get; private set; }

        public static IKernelMemory Create(InvocationHandler handler)
        {
            var proxy = DispatchProxy.Create<IKernelMemory, KernelMemoryDispatchProxy>();
            ((KernelMemoryDispatchProxy)(object)proxy!).Handler = handler;
            return proxy!;
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            if (Handler == null || targetMethod == null || args == null)
            {
                throw new NotImplementedException(targetMethod?.Name ?? nameof(Invoke));
            }

            return Handler(targetMethod, args);
        }
    }
}
