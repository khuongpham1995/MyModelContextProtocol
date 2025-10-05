using FluentAssertions;
using McpServer.Application.Handlers.Document;
using McpServer.Domain.Interfaces;
using Moq;
using DocumentType = McpServer.Application.Handlers.Document.Document;

namespace McpServer.Tests.Application.Handlers.Document;

public class AskBackendQuestionHandlerTests
{
    [Fact]
    public async Task Handle_WhenDocumentIsReady_ReturnsKernelAnswer()
    {
        var documentPath = EnsureDocumentExists("backend_structure.md");

        var kernelMock = new Mock<IKernelMemoryService>();
        const string question = "Describe the backend architecture.";
        const string expectedAnswer = "It follows a modular design.";

        kernelMock
            .Setup(x => x.IsDocumentReadyAsync("backend_structure", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        kernelMock
            .Setup(x => x.AskAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAnswer);

        var handler = new AskBackendQuestionHandler(kernelMock.Object);
        var request = new AskBackendQuestionCommand(question);

        try
        {
            var result = await handler.Handle(request, CancellationToken.None);

            result.Should().Be(expectedAnswer);
            kernelMock.Verify(x => x.UploadDocumentAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            kernelMock.Verify(x => x.IsDocumentReadyAsync("backend_structure", It.IsAny<CancellationToken>()), Times.Once);
            kernelMock.Verify(
                x => x.AskAsync(
                    It.Is<string>(prompt => prompt.Contains(question) && prompt.Contains("Backend Structure")),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
        finally
        {
            CleanupDocument(documentPath);
        }
    }

    [Fact]
    public async Task Handle_WhenDocumentNotReady_UploadsDocumentBeforeAnswering()
    {
        var documentPath = EnsureDocumentExists("backend_tests.md");

        var kernelMock = new Mock<IKernelMemoryService>();
        const string question = "How should backend unit tests look?";
        const string expectedAnswer = "Apply Arrange-Act-Assert.";

        var readiness = new Queue<bool>(new[] { false, true });

        kernelMock
            .Setup(x => x.IsDocumentReadyAsync("backend_tests", It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(readiness.Dequeue()));
        kernelMock
            .Setup(x => x.UploadDocumentAsync(documentPath, "backend_tests", It.IsAny<CancellationToken>()))
            .ReturnsAsync("uploaded");
        kernelMock
            .Setup(x => x.AskAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedAnswer);

        var handler = new AskBackendQuestionHandler(kernelMock.Object);
        var request = new AskBackendQuestionCommand(question, DocumentType.BackendTests);

        try
        {
            var result = await handler.Handle(request, CancellationToken.None);

            result.Should().Be(expectedAnswer);
            kernelMock.Verify(x => x.IsDocumentReadyAsync("backend_tests", It.IsAny<CancellationToken>()), Times.AtLeast(2));
            kernelMock.Verify(x => x.UploadDocumentAsync(documentPath, "backend_tests", It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            kernelMock.Verify(
                x => x.AskAsync(
                    It.Is<string>(prompt => prompt.Contains(question) && prompt.Contains("Backend Tests")),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
        finally
        {
            CleanupDocument(documentPath);
        }
    }

    [Fact]
    public async Task Handle_WhenDocumentMissing_ThrowsFileNotFoundException()
    {
        var documentPath = Path.Combine(GetDocumentsDirectory(), "backend_structure.md");
        if (File.Exists(documentPath))
        {
            File.Delete(documentPath);
        }

        var kernelMock = new Mock<IKernelMemoryService>();
        var handler = new AskBackendQuestionHandler(kernelMock.Object);
        var request = new AskBackendQuestionCommand("Any question");

        var act = async () => await handler.Handle(request, CancellationToken.None);

        await act.Should().ThrowAsync<FileNotFoundException>()
            .WithMessage($"File not found: {documentPath}");
    }

    private static string EnsureDocumentExists(string fileName)
    {
        var documentsDirectory = GetDocumentsDirectory();
        Directory.CreateDirectory(documentsDirectory);
        var filePath = Path.Combine(documentsDirectory, fileName);
        File.WriteAllText(filePath, "# Test Document\nContent");
        return filePath;
    }

    private static void CleanupDocument(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private static string GetDocumentsDirectory() => Path.Combine(AppContext.BaseDirectory, "Documents");
}
