namespace MonoAccountProvider.Services.Models;

public record AccountDto(
	decimal Balance,
	int CurrencyCode,
	IList<string> MaskedPan);