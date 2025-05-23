using McpServer.Domain.Constants;
using Microsoft.Azure.Functions.Worker.Builder;

namespace McpServer.Function.McpTools;

public static class ToolExtension
{
    public static void RegisterMcpTools(this FunctionsApplicationBuilder builder)
    {
        builder
            .ConfigureMcpTool(CommonConstant.AskBackendDocumentToolName)
            .WithProperty(CommonConstant.QuestionPropertyName, CommonConstant.PropertyTypeString, CommonConstant.QuestionPropertyDescription);
    }
}
