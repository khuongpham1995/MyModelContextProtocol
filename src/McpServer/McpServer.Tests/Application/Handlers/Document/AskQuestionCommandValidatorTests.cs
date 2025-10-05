using FluentAssertions;
using McpServer.Application.Handlers.Document;

namespace McpServer.Tests.Application.Handlers.Document;

public class AskQuestionCommandValidatorTests
{
    private readonly AskQuestionCommandValidator _validator = new();

    [Fact]
    public void Validate_WhenQuestionProvided_IsValid()
    {
        var result = _validator.Validate(new AskBackendQuestionCommand("Explain the services layer."));

        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WhenQuestionMissing_ReturnsValidationError()
    {
        var result = _validator.Validate(new AskBackendQuestionCommand(""));

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(error => error.ErrorMessage == "Question is required.");
    }
}
