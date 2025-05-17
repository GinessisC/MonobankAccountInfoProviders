namespace MonoAccountProvider.Domain.Entities;

public record UserAccountInCurrencies(
	string MaskedPan,
	IAsyncEnumerable<MoneyWithNamedCurrency> Balance);