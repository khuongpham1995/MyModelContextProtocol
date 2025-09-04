using System.ComponentModel;
using FluentValidation;
using McpServer.Application.KernelMemoryHandlers;
using McpServer.Domain.Constants;
using McpServer.Domain.Models;
using MediatR;
using ModelContextProtocol.Server;

namespace McpServer.WebService.McpTools;

[McpServerToolType]
public sealed class DocumentTool(
    ISender sender,
    ILogger<DocumentTool> logger)
{
    [McpServerTool(Name = CommonConstant.AskBackendStructureDocumentToolName),
     Description(CommonConstant.AskBackendStructureDocumentToolDescription)]
    public async Task<ResponseModel<string>> AskBackendStructureAsync(
        [Description(CommonConstant.QuestionPropertyDescription)] string? question,
        CancellationToken ct = default)
    {
        var response = new ResponseModel<string>();

        try
        {
            var result = await sender.Send(new AskBackendQuestionCommand(question!), ct);
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

    [McpServerTool(Name = CommonConstant.AskBackendTestsDocumentToolName),
     Description(CommonConstant.AskBackendTestsDocumentToolDescription)]
    public async Task<ResponseModel<string>> AskBackendUnitTestsAsync(
        [Description(CommonConstant.QuestionPropertyDescription)] string? question,
        CancellationToken ct = default)
    {
        var response = new ResponseModel<string>();
        try
        {
            var result = await sender.Send(new AskBackendQuestionCommand(question!, Document.BackendTests), ct);
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
