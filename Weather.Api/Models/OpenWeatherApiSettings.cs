namespace Weather.Api.Models;

public class OpenWeatherApiSettings
{
    public const string Key = "OpenWeatherMapApi";

    public string ApiKey { get; set; } = default!;
}
