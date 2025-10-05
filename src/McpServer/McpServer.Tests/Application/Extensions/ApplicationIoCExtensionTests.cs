using FluentAssertions;
using FluentValidation;
using McpServer.Application.Behaviors;
using McpServer.Application.Extensions;
using McpServer.Application.Handlers.Document;
using McpServer.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace McpServer.Tests.Application.Extensions;

public class ApplicationIoCExtensionTests
{
    [Fact]
    public void AddApplication_RegistersValidatorsAndPipelineBehavior()
    {
        var services = new ServiceCollection();

        services.AddApplication();
        services.AddSingleton<IKernelMemoryService, KernelMemoryServiceStub>();

        using var provider = services.BuildServiceProvider();
        var validators = provider.GetServices<IValidator<AskBackendQuestionCommand>>();
        validators.Should().ContainSingle(v => v is AskQuestionCommandValidator);

        var behaviors = provider.GetServices<IPipelineBehavior<AskBackendQuestionCommand, string>>();
        behaviors.Should().ContainSingle(b => b is ValidationBehavior<AskBackendQuestionCommand, string>);
    }

    [Fact]
    public void AddApplication_ResolvesRequestHandlerFromAssembly()
    {
        var services = new ServiceCollection();

        services.AddApplication();
        services.AddSingleton<IKernelMemoryService, KernelMemoryServiceStub>();

        using var provider = services.BuildServiceProvider();
        var handler = provider.GetRequiredService<IRequestHandler<AskBackendQuestionCommand, string>>();

        handler.Should().BeOfType<AskBackendQuestionHandler>();
    }

    private sealed class KernelMemoryServiceStub : IKernelMemoryService
    {
        public Task<string> AskAsync(string question, CancellationToken cancellationToken = default) => Task.FromResult(string.Empty);

        public Task<string> UploadDocumentAsync(string filePath, string documentId, CancellationToken cancellationToken = default) => Task.FromResult(string.Empty);

        public Task<bool> IsDocumentReadyAsync(string documentId, CancellationToken cancellationToken = default) => Task.FromResult(true);
    }
}
