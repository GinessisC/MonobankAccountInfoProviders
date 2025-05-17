namespace MonoAccountProvider.Domain.Entities;

public record Money(
	decimal Amount,
	int CurrencyCode);