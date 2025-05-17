namespace MonoAccountProvider.Domain.Entities;

public record UserConfig(
	string Token,
	IEnumerable<string> CurrencyNames);