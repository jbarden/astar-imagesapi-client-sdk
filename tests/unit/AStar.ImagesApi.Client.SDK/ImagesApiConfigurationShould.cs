using AStar.ImagesApi.Client.SDK.ImagesApi;
using FluentAssertions;

namespace AStar.ImagesApi.Client.SDK;

public class ImagesApiConfigurationShould
{
    [Fact]
    public void ReturnTheExpectedDefaultValue() => new ImagesApiConfiguration().BaseUrl.Should().Be("http://not.set.com/");

    [Fact]
    public void ReturnTheExpectedSectionLocationValue() => ImagesApiConfiguration.SectionLocation.Should().Be("ApiConfiguration:ImagesApiConfiguration");
}
