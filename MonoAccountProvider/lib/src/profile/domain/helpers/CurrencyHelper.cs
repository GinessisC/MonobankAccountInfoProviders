using MonoAccountProvider.lib.src.profile.domain.cache.Interfaces;
using MonoAccountProvider.lib.src.profile.domain.Entities;
using MonoAccountProvider.lib.src.profile.domain.helpers.Interfaces;
using MonoAccountProvider.lib.src.profile.domain.usecases;

namespace MonoAccountProvider.lib.src.profile.domain.helpers;

public class CurrencyHelper : ICurrencyHelper
{
	private readonly IList<Currency> _currencies;
	
	public CurrencyHelper(ICurrencyInfoCache cache)
	{
		_currencies = cache.Currencies;
	}
	public IList<int> CurrencyCodesThatContainCurrencyNames(string[] names)
	{
		return _currencies
			.Where(c => names.Contains(c.Name))
			.Select(c => c.Code)
			.ToList();
	}

	public IList<MoneyWithNamedCurrency> ToMoneyWithNamedCurrency(IList<Money> m)
	{
		CurrencyInfoConverter currencyConverter = new(_currencies);
		List<MoneyWithNamedCurrency> moneyWithNamedCurrencies = [];
			
		foreach (var money in m)
			moneyWithNamedCurrencies.Add(currencyConverter.ToMoneyWithNamedCurrency(money));

		return moneyWithNamedCurrencies;
	}
}