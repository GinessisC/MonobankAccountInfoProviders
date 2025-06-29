namespace MonoAccountProvider.Services.Models;

public record JarDto(
	string Title,
	decimal Balance,
	int CurrencyCode);