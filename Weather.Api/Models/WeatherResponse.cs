using System.Text.Json.Serialization;

namespace Weather.Api.Models;

public class WeatherResponse
{
    [JsonPropertyName("id")]
    public long Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = default!;
    
    [JsonPropertyName("weather")]
    public List<WeatherDescription> WeatherDescription { get; init; } = default!;

    [JsonPropertyName("base")]
    public string Base { get; init; } = default!;

    [JsonPropertyName("visibility")]
    public long Visibility { get; init; }

    [JsonPropertyName("timezone")]
    public long Timezone { get; init; }
}


