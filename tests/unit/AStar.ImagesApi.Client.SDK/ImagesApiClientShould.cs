using AStar.ImagesApi.Client.SDK.ImagesApi;
using AStar.ImagesApi.Client.SDK.MockMessageHandlers;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;

namespace AStar.ImagesApi.Client.SDK;

public class ImagesApiClientShould
{
    [Fact]
    public async Task ReturnExpectedFailureFromGetHealthAsyncWhenTheApIsiUnreachable()
    {
        var handler = new MockHttpRequestExceptionErrorHttpMessageHandler();

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new("https://doesnot.matter.com")
        };

        var sut = new ImagesApiClient(httpClient, NullLogger<ImagesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Could not get a response from the Images API.");
    }

    [Fact]
    public async Task ReturnExpectedFailureMessageFromGetHealthAsyncWhenCheckFails()
    {
        var handler = new MockInternalServerErrorHttpMessageHandler("Health Check failed.");

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new("https://doesnot.matter.com")
        };

        var sut = new ImagesApiClient(httpClient, NullLogger<ImagesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("Health Check failed.");
    }

    [Fact]
    public async Task ReturnExpectedMessageFromGetHealthAsyncWhenCheckSucceeds()
    {
        var handler = new MockSuccessHttpMessageHandler("");

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new("https://doesnot.matter.com")
        };

        var sut = new ImagesApiClient(httpClient, NullLogger<ImagesApiClient>.Instance);

        var response = await sut.GetHealthAsync();

        response.Status.Should().Be("OK");
    }
}
