using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountProvider.lib.src.profile.domain.helpers.Interfaces;

public interface ICurrencyHelper
{
	IList<int> CurrencyCodesThatContainCurrencyNames(string[] names);
	IList<MoneyWithNamedCurrency> ToMoneyWithNamedCurrency(IList<Money> m);
}