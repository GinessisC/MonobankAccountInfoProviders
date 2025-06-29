using Domain.Entities;

namespace MonoAccountProvider.Domain.Helpers.Interfaces;

public interface ICurrencyOperator
{
	IAsyncEnumerable<int> ToCurrencyCodes(IEnumerable<string> names);
	IAsyncEnumerable<MoneyWithNamedCurrency> ToMoneyWithNamedCurrency(IAsyncEnumerable<Money> moneyAsyncEnumerable);
}