using System.Net;

namespace AStar.ImagesApi.Client.SDK.MockMessageHandlers;

public class MockInternalServerErrorHttpMessageHandler(string errorMessage) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(errorMessage)
        });
}
