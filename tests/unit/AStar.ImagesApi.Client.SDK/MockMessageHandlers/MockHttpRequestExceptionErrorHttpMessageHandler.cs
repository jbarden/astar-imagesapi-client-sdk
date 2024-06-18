namespace AStar.ImagesApi.Client.SDK.MockMessageHandlers;

public class MockHttpRequestExceptionErrorHttpMessageHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        => throw new HttpRequestException();
}
