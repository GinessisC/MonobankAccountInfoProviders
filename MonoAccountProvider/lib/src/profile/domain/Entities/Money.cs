namespace MonoAccountProvider.lib.src.profile.domain.Entities;

public class Money
{
	public decimal Amount { get; }
	public int CurrencyCode { get; }

	public Money(decimal amount, int currencyCode)
	{
		Amount = amount;
		CurrencyCode = currencyCode;
	}
}