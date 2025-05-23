using McpServer.Domain.Constants;
using Microsoft.Azure.Functions.Worker.Builder;

namespace McpServer.Function.McpTools;

public static class ToolExtension
{
    public static void RegisterMcpTools(this FunctionsApplicationBuilder builder)
    {
        builder
            .ConfigureMcpTool(CommonConstant.AskBackendStructureDocumentToolName)
            .WithProperty(CommonConstant.QuestionPropertyName, CommonConstant.PropertyTypeString, CommonConstant.QuestionPropertyDescription);

            builder
            .ConfigureMcpTool(CommonConstant.AskBackendTestsDocumentToolName)
            .WithProperty(CommonConstant.QuestionPropertyName, CommonConstant.PropertyTypeString, CommonConstant.QuestionPropertyDescription);
    }
}
