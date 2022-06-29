using System.Text.Json.Serialization;

namespace RPSSL.Infrastructure.Services;

public class GetRandomResponse
{
    [JsonPropertyName("random_number")]
    public int RandomNumber { get; set; }
}
