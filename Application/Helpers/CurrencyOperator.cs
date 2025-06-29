using Application.Repositories;
using Domain.Entities;
using Domain.Extensions;
using MonoAccountProvider.Domain.Helpers.Interfaces;
using MonoAccountProvider.Domain.Repositories;

namespace Application.Helpers;

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

	public async IAsyncEnumerable<MoneyWithNamedCurrency> ToMoneyWithNamedCurrency(
		IAsyncEnumerable<Money> moneyAsyncEnumerable)
	{
		await foreach (Money money in moneyAsyncEnumerable)
		{
			MoneyWithNamedCurrency moneyWithNamedCurrency = await money.ToMoneyWithNamedCurrencyAsync(_currencies);

			yield return moneyWithNamedCurrency;
		}
	}
}