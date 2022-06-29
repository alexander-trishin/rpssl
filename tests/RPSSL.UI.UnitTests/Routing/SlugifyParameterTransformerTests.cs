using FluentAssertions;

using RPSSL.UI.Routing;

namespace RPSSL.UI.UnitTests.Routing;

public class SlugifyParameterTransformerTests
{
    public static IEnumerable<object[]> Entries
    {
        get
        {
            yield return new string[] { "CamelCase", "camel-case" };
            yield return new string[] { "DynamoDB", "dynamo-db" };
            yield return new string[] { string.Empty, string.Empty };
            yield return new string[] { null, null };
        }
    }

    [Theory]
    [MemberData(nameof(Entries))]
    public void TransformOutbound_WhenInputStringProvided_ThenReturnsSlugifiedVersion(string input, string expected)
    {
        // Arrange
        var transformer = new SlugifyParameterTransformer();

        // Act
        var actual = transformer.TransformOutbound(input);

        // Assert
        actual.Should().Be(expected);
    }
}

