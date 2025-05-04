namespace MonoAccountProvider.lib.src.profile.domain.Entities;

public class MoneyWithNamedCurrency
{
	public decimal Amount { get; }
	public string CurrencyName { get; }

	public MoneyWithNamedCurrency(decimal amount, string currencyName)
	{
		Amount = amount;
		CurrencyName = currencyName;
	}
}