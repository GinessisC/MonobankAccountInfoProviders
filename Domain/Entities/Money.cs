namespace Domain.Entities;

public record Money(
	decimal Amount,
	int CurrencyCode);