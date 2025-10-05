using System.Net.Http;
using FluentAssertions;
using McpServer.Domain.Interfaces;
using McpServer.Infrastructure.ApiClients;
using McpServer.Infrastructure.Extensions;
using McpServer.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.KernelMemory;

namespace McpServer.Tests.Infrastructure.Extensions;

public class IoCExtensionTests
{
    [Fact]
    public void AddInfrastructure_RegistersDependencies()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["MockApi:BaseUrl"] = "https://mock-api.test",
            ["KernelMemory:BaseUrl"] = "https://kernel-memory.test"
        });

        var services = new ServiceCollection();

        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();
        var customerApi = provider.GetRequiredService<ICustomerApiClient>();
        var kernelService = provider.GetRequiredService<IKernelMemoryService>();
        var kernelClient = provider.GetRequiredService<IKernelMemory>();

        customerApi.Should().BeOfType<CustomerApiClient>();
        kernelService.Should().BeOfType<KernelMemoryService>();
        kernelClient.Should().NotBeNull();

        var httpClientField = typeof(CustomerApiClient)
            .GetField("_httpClient", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        httpClientField.Should().NotBeNull();
        var httpClient = httpClientField!.GetValue(customerApi) as HttpClient;
        httpClient.Should().NotBeNull();
        httpClient!.BaseAddress.Should().Be(configuration.GetValue<Uri>("MockApi:BaseUrl"));
        httpClient.DefaultRequestHeaders.Accept.Should().ContainSingle(
            value => value.MediaType == "application/json");
    }

    [Fact]
    public void AddInfrastructure_WhenMockApiBaseUrlMissing_Throws()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["KernelMemory:BaseUrl"] = "https://kernel-memory.test"
        });
        var services = new ServiceCollection();

        var act = () =>
        {
            services.AddInfrastructure(configuration);
            using var provider = services.BuildServiceProvider();
            provider.GetRequiredService<ICustomerApiClient>();
        };

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*MockApi:BaseUrl*");
    }

    [Fact]
    public void AddInfrastructure_WhenKernelMemoryBaseUrlMissing_Throws()
    {
        var configuration = BuildConfiguration(new Dictionary<string, string?>
        {
            ["MockApi:BaseUrl"] = "https://mock-api.test"
        });
        var services = new ServiceCollection();

        var act = () => services.AddInfrastructure(configuration);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*KernelMemory:BaseUrl*");
    }

    private static IConfiguration BuildConfiguration(IDictionary<string, string?> values)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(values)
            .Build();
    }
}
