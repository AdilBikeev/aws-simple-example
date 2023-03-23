using System.Text.Json.Serialization;

namespace Customers.Api.Contracts.Data;

public class CustomerDto
{
    [JsonPropertyName("pk")]
    public Guid Pk { get; init; }
    
    [JsonPropertyName("sk")]
    public Guid Sk { get; init; }
    
    public Guid Id { get; init; } = default!;

    public string GitHubUsername { get; init; } = default!;

    public string FullName { get; init; } = default!;

    public string Email { get; init; } = default!;

    public DateTime DateOfBirth { get; init; }
    
    public DateTime UpdateAt { get; set; }
}