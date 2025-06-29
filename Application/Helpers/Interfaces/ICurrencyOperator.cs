using Domain.Entities;

namespace Application.Helpers.Interfaces;

public interface ICurrencyOperator
{
	IAsyncEnumerable<int> ToCurrencyCodes(IEnumerable<string> names);
	IAsyncEnumerable<MoneyWithNamedCurrency> ToMoneyWithNamedCurrency(IAsyncEnumerable<Money> moneyAsyncEnumerable);
}