using System.Text.Json.Serialization;

namespace Weather.Api.Models;

public class WeatherDescription
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("main")]
    public string Main { get; init; } = default!;

    [JsonPropertyName("description")]
    public string Description { get; init; } = default!;

    [JsonPropertyName("icon")]
    public string Icon { get; init; } = default!;
}
