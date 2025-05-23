using McpServer.Infrastructure.Extensions;
using McpServer.Application.Extensions;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using McpServer.Function.McpTools;
using Microsoft.Extensions.DependencyInjection;

var builder = FunctionsApplication.CreateBuilder(args);
builder.ConfigureFunctionsWebApplication();
builder.EnableMcpToolMetadata();
builder.Configuration.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddLogging();
builder.RegisterMcpTools();

await builder.Build().RunAsync();