using MonoAccountProvider.Domain.Entities;

namespace MonoAccountProvider.Domain.helpers.Interfaces;

public interface ICurrencyOperator
{
	IAsyncEnumerable<int> ToCurrencyCodes(IEnumerable<string> names);
	IAsyncEnumerable<MoneyWithNamedCurrency> ToMoneyWithNamedCurrency(IAsyncEnumerable<Money> moneyAsyncEnumerable);
}