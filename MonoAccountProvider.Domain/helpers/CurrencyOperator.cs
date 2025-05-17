using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.helpers.Interfaces;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Domain.Extensions; 

namespace MonoAccountProvider.Domain.helpers;

public class CurrencyOperator : ICurrencyOperator
{
	private readonly IAsyncEnumerable<Currency> _currencies;
	
	public CurrencyOperator(ICurrencyInfoRepository currencyRepository)
	{
		_currencies = currencyRepository.GetAllCurrenciesAsync();
	}
	
	public IAsyncEnumerable<int> ToCurrencyCodes(IEnumerable<string> names)
	{
		return _currencies
			.Where(c => names.Contains(c.Name))
			.Select(c => c.Code);
	}
	
	public async IAsyncEnumerable<MoneyWithNamedCurrency> ToMoneyWithNamedCurrency(IAsyncEnumerable<Money> moneyAsyncEnumerable)
	{
		await foreach (var money in moneyAsyncEnumerable)
		{
			var moneyWithNamedCurrency = await money.ToMoneyWithNamedCurrencyAsync(_currencies);
			
			yield return moneyWithNamedCurrency;
		}
	}
}