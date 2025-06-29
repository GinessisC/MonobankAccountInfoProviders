using System.Text.Json.Serialization;

namespace MonoAccountProvider.Services.Models;

public record CurrencyDto(
	[property: JsonPropertyName("cc")]string Name,
	[property: JsonPropertyName("r030")]int CurrencyCode);