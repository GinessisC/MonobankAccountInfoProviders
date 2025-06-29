namespace Domain.Entities;

public record MoneyWithNamedCurrency(
	decimal Amount,
	string CurrencyName);