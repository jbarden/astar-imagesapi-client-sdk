using System.Text.Json;
using AStar.Api.HealthChecks;
using Microsoft.Extensions.Logging;

namespace AStar.ImagesApi.Client.SDK.ImagesApi;

/// <summary>
/// The <see href="ImagesApiClient"></see> class.
/// </summary>
/// <param name="httpClient"></param>
/// <param name="logger"></param>
public class ImagesApiClient(HttpClient httpClient, ILogger<ImagesApiClient> logger) : IApiClient
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    /// <inheritdoc/>
    public async Task<HealthStatusResponse> GetHealthAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("/health/live");

            return response.IsSuccessStatusCode
                ? (await JsonSerializer.DeserializeAsync<HealthStatusResponse>(await response.Content.ReadAsStreamAsync(), JsonSerializerOptions))!
                : new() { Status = "Health Check failed." }!;
        }
        catch(HttpRequestException ex)
        {
            logger.LogError(500, ex, "Error: {ErrorMessage}", ex.Message);
            return new() { Status = "Could not get a response from the Images API." }!;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="imagePath"></param>
    /// <param name="maxDimensions"></param>
    /// <param name="thumbnail"></param>
    /// <returns></returns>
    public async Task<Stream> GetImageAsync(string imagePath, int maxDimensions, bool thumbnail)
    {
        var requestUri = $"api/image?thumbnail={thumbnail}&imagePath={Uri.EscapeDataString(imagePath)}&maxDimensions={maxDimensions}";
        var response = await httpClient.GetAsync(requestUri);

        return response.IsSuccessStatusCode
            ? await response.Content.ReadAsStreamAsync()
            : CreateNotFoundMemoryStream(imagePath);
    }

    private MemoryStream CreateNotFoundMemoryStream(string fileName)
    {
        logger.LogInformation("Could not find: {FileName}", fileName);

        return new(Models.NotFound.Image);
    }
}
