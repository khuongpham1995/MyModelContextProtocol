using FluentAssertions;
using FluentValidation;
using McpServer.Application.Handlers.Document;
using McpServer.Domain.Constants;
using McpServer.Presentation.McpTools;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace McpServer.Tests.McpTools;

public class DocumentToolTests
{
    [Fact]
    public async Task AskBackendStructureAsync_ReturnsSuccessResponse()
    {
        const string question = "How is the backend structured?";
        const string expectedAnswer = "The backend follows a clean architecture.";
        AskBackendQuestionCommand? capturedCommand = null;

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<IRequest<string>>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<string>, CancellationToken>((request, _) => capturedCommand = request as AskBackendQuestionCommand)
            .ReturnsAsync(expectedAnswer);

        var loggerMock = new Mock<ILogger<DocumentTool>>();
        var tool = new DocumentTool(senderMock.Object, loggerMock.Object);

        var response = await tool.AskBackendStructureAsync(question);

        response.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(expectedAnswer);
        response.Error.Should().BeEmpty();

        capturedCommand.Should().NotBeNull();
        capturedCommand!.Question.Should().Be(question);
        capturedCommand.Type.Should().Be(Document.BackendStructure);
    }

    [Fact]
    public async Task AskBackendStructureAsync_WhenValidationException_ReturnsFailure()
    {
        const string question = "";
        const string validationMessage = "Question is required.";

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<IRequest<string>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException(validationMessage));

        var loggerMock = new Mock<ILogger<DocumentTool>>();
        var tool = new DocumentTool(senderMock.Object, loggerMock.Object);

        var response = await tool.AskBackendStructureAsync(question);

        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
        response.Error.Should().Be(validationMessage);
    }

    [Fact]
    public async Task AskBackendStructureAsync_WhenFileNotFound_ReturnsFailure()
    {
        const string question = "Where are the docs?";
        const string fileMessage = "File not found";

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<IRequest<string>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new FileNotFoundException(fileMessage));

        var loggerMock = new Mock<ILogger<DocumentTool>>();
        var tool = new DocumentTool(senderMock.Object, loggerMock.Object);

        var response = await tool.AskBackendStructureAsync(question);

        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
        response.Error.Should().Be(fileMessage);
    }

    [Fact]
    public async Task AskBackendStructureAsync_WhenUnhandledException_ReturnsGenericFailure()
    {
        const string question = "Trigger error";

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<IRequest<string>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Unexpected"));

        var loggerMock = new Mock<ILogger<DocumentTool>>();
        var tool = new DocumentTool(senderMock.Object, loggerMock.Object);

        var response = await tool.AskBackendStructureAsync(question);

        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
        response.Error.Should().Be(CommonConstant.ErrorAskQuestionDocumentFailed);
    }

    [Fact]
    public async Task AskBackendUnitTestsAsync_ReturnsSuccessResponse()
    {
        const string question = "How should unit tests be structured?";
        const string expectedAnswer = "Use Arrange-Act-Assert structure.";
        AskBackendQuestionCommand? capturedCommand = null;

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<IRequest<string>>(), It.IsAny<CancellationToken>()))
            .Callback<IRequest<string>, CancellationToken>((request, _) => capturedCommand = request as AskBackendQuestionCommand)
            .ReturnsAsync(expectedAnswer);

        var loggerMock = new Mock<ILogger<DocumentTool>>();
        var tool = new DocumentTool(senderMock.Object, loggerMock.Object);

        var response = await tool.AskBackendUnitTestsAsync(question);

        response.IsSuccess.Should().BeTrue();
        response.Data.Should().Be(expectedAnswer);
        response.Error.Should().BeEmpty();

        capturedCommand.Should().NotBeNull();
        capturedCommand!.Question.Should().Be(question);
        capturedCommand.Type.Should().Be(Document.BackendTests);
    }

    [Fact]
    public async Task AskBackendUnitTestsAsync_WhenValidationException_ReturnsFailure()
    {
        const string question = "";
        const string validationMessage = "Unit test question missing";

        var senderMock = new Mock<ISender>();
        senderMock
            .Setup(x => x.Send(It.IsAny<IRequest<string>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ValidationException(validationMessage));

        var loggerMock = new Mock<ILogger<DocumentTool>>();
        var tool = new DocumentTool(senderMock.Object, loggerMock.Object);

        var response = await tool.AskBackendUnitTestsAsync(question);

        response.IsSuccess.Should().BeFalse();
        response.Data.Should().BeNull();
        response.Error.Should().Be(validationMessage);
    }
}
