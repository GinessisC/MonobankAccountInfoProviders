namespace Domain.Entities;

public record UserJarInCurrencies(
	string Title,
	IAsyncEnumerable<MoneyWithNamedCurrency> Balance);