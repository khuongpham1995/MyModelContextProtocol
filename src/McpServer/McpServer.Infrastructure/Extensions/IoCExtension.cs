using System.Net.Http.Headers;
using McpServer.Domain.Interfaces;
using McpServer.Infrastructure.ApiClients;
using McpServer.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.KernelMemory;

namespace McpServer.Infrastructure.Extensions;

public static class IoCExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ICustomerApiClient, CustomerApiClient>(client =>
        {
            var domain = configuration["MockApi:BaseUrl"] ?? throw new ArgumentNullException(nameof(configuration),
                "Configuration key 'MockApi:BaseUrl' is missing");
            client.BaseAddress = new Uri(domain);
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        var kernelMemoryApiUrl = configuration["KernelMemory:BaseUrl"] ??
                                 throw new ArgumentNullException(nameof(configuration),
                                     "Configuration key 'KernelMemory:BaseUrl' is missing");
        services.AddSingleton<IKernelMemory>(_ => new MemoryWebClient(kernelMemoryApiUrl));
        services.AddScoped<IKernelMemoryService, KernelMemoryService>();
    }
}