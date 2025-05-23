using FluentValidation;
using McpServer.Application.KernelMemoryHandlers;
using McpServer.Domain.Constants;
using McpServer.Domain.Models;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace McpServer.Function.McpTools;

public class DocumentTool(
    ISender sender,
    ILogger<DocumentTool> logger)
{
    [Function(nameof(CommonConstant.AskBackendStructureDocumentToolName))]
    public async Task<ResponseModel<string>> AskBackendStructureAsync(
        [McpToolTrigger(CommonConstant.AskBackendStructureDocumentToolName, CommonConstant.AskBackendStructureDocumentToolDescription)] ToolInvocationContext context,
        [McpToolProperty(CommonConstant.QuestionPropertyName, CommonConstant.PropertyTypeString, CommonConstant.QuestionPropertyDescription)] string question)
    {
        var response = new ResponseModel<string>();
        try
        {
            var result = await sender.Send(new AskBackendQuestionCommand(question));
            response.Succeed(result);
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, CommonConstant.ErrorAskQuestionDocumentFailed);
            response.Fail(ex.Message);
        }
        catch (FileNotFoundException ex)
        {
            logger.LogError(ex, CommonConstant.ErrorAskQuestionDocumentFailed);
            response.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, CommonConstant.ErrorAskQuestionDocumentFailed);
            response.Fail(CommonConstant.ErrorAskQuestionDocumentFailed);
        }

        return response;
    }

    [Function(nameof(CommonConstant.AskBackendTestsDocumentToolName))]
    public async Task<ResponseModel<string>> AskBackendUnitTestsAsync(
        [McpToolTrigger(CommonConstant.AskBackendTestsDocumentToolName, CommonConstant.AskBackendTestsDocumentToolDescription)] ToolInvocationContext context,
        [McpToolProperty(CommonConstant.QuestionPropertyName, CommonConstant.PropertyTypeString, CommonConstant.QuestionPropertyDescription)] string question)
    {
        var response = new ResponseModel<string>();
        try
        {
            var result = await sender.Send(new AskBackendQuestionCommand(question, Document.BackendTests));
            response.Succeed(result);
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex, CommonConstant.ErrorAskQuestionDocumentFailed);
            response.Fail(ex.Message);
        }
        catch (FileNotFoundException ex)
        {
            logger.LogError(ex, CommonConstant.ErrorAskQuestionDocumentFailed);
            response.Fail(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, CommonConstant.ErrorAskQuestionDocumentFailed);
            response.Fail(CommonConstant.ErrorAskQuestionDocumentFailed);
        }

        return response;
    }
}
