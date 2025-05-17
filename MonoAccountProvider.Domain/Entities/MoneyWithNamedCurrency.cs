namespace MonoAccountProvider.Domain.Entities;

public record MoneyWithNamedCurrency(
	decimal Amount,
	string CurrencyName);