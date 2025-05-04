using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.domain.usecases;

public class CurrencyInfoConverter
{
	private readonly IList<Currency> _currencies;

	public CurrencyInfoConverter(IList<Currency> currencies)
	{
		_currencies = currencies;
	}

	public MoneyWithNamedCurrency ToMoneyWithNamedCurrency(Money money)
	{
		string? currencyName = _currencies
			.Where(c => c.Code == money.CurrencyCode)
			.Select(c => c.Name)
			.FirstOrDefault();

		if (currencyName is null)
		{
			throw new FormatException("No currency found");
		}

		return new MoneyWithNamedCurrency(money.Amount, currencyName);
	}
}