using System.Net;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using McpServer.Domain.Entities;
using McpServer.Infrastructure.ApiClients;

namespace McpServer.Tests.Infrastructure.ApiClients;

public class CustomerApiClientTests
{
    [Fact]
    public async Task GetByNameAsync_ReturnsCustomersAndBuildsQuery()
    {
        var customers = new List<Customer>
        {
            new() { Id = 1, Name = "Alice" }
        };

        var handler = new StubHttpMessageHandler(customers);
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.test")
        };
        var client = new CustomerApiClient(httpClient);

        var result = await client.GetByNameAsync("alice", CancellationToken.None);

        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Alice");
        handler.LastRequest.Should().NotBeNull();
        handler.LastRequest!.Method.Should().Be(HttpMethod.Get);
        handler.LastRequest.RequestUri!.PathAndQuery.Should().Be("/api/v1/customer?name=alice");
        handler.LastRequest.Headers.Accept.Should().BeEmpty();
    }

    [Fact]
    public async Task CreateAsync_PostsEntityAndReturnsCreatedCustomer()
    {
        var newCustomer = new Customer { Id = 1, Name = "Alice" };
        var handler = new StubHttpMessageHandler(newCustomer)
        {
            ResponseStatusCode = HttpStatusCode.Created
        };
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.test")
        };
        var client = new CustomerApiClient(httpClient);

        var response = await client.CreateAsync(newCustomer, CancellationToken.None);

        response.Id.Should().Be(1);
        handler.LastRequest.Should().NotBeNull();
        handler.LastRequest!.Method.Should().Be(HttpMethod.Post);
        handler.LastRequest.RequestUri!.AbsolutePath.Should().Be("/api/v1/customer");
    }

    [Fact]
    public async Task UpdateAsync_WhenRequestFails_ThrowsHttpRequestException()
    {
        var handler = new StubHttpMessageHandler(null)
        {
            ResponseStatusCode = HttpStatusCode.BadRequest
        };
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.test")
        };
        var client = new CustomerApiClient(httpClient);

        var act = async () => await client.UpdateAsync(2, new Customer { Id = 2 }, CancellationToken.None);

        await act.Should().ThrowAsync<HttpRequestException>();
        handler.LastRequest.Should().NotBeNull();
        handler.LastRequest!.Method.Should().Be(HttpMethod.Put);
        handler.LastRequest.RequestUri!.PathAndQuery.Should().Be("/api/v1/customer/2");
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly object? _responseModel;

        public HttpRequestMessage? LastRequest { get; private set; }
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.OK;
        public string MediaType { get; set; } = "application/json";

        public StubHttpMessageHandler(object? responseModel)
        {
            _responseModel = responseModel;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            var response = new HttpResponseMessage(ResponseStatusCode);
            if (_responseModel != null)
            {
                var payload = JsonSerializer.Serialize(_responseModel);
                response.Content = new StringContent(payload, Encoding.UTF8, MediaType);
            }
            else
            {
                response.Content = new StringContent(string.Empty, Encoding.UTF8, MediaType);
            }

            return Task.FromResult(response);
        }
    }
}
