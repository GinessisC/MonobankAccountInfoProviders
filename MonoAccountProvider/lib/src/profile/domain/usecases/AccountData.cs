using MonoAccountProvider.lib.src.profile.domain.cache.Interfaces;
using MonoAccountProvider.lib.src.profile.domain.Entities;
using MonoAccountProvider.lib.src.profile.domain.helpers;
using MonoAccountProvider.lib.src.profile.domain.helpers.Interfaces;

namespace MonoAccountProvider.lib.src.profile.domain.usecases;

public class AccountData : IAccountData
{
	private readonly IList<Account> _accounts;
	private readonly IList<CurrencyRate> _rates;
	private readonly IList<int> _currencyCodesToConvertBalanceTo;
	private readonly ICurrencyHelper _currencyHelper;
	public AccountData(ICurrencyInfoCache currencyCache, IUserCache userCache, ICurrencyHelper helper)
	{
		_currencyHelper = helper;
		_rates = currencyCache.Rates;
		_accounts = userCache.Profile.Accounts;
		
		
		_currencyCodesToConvertBalanceTo =
			helper.CurrencyCodesThatContainCurrencyNames(userCache.Conf.CurrencyNames);
	}

	public IList<UserAccountInCurrencies> GetAccounts()
	{
		List<UserAccountInCurrencies> balances = new();

		foreach (Account account in _accounts)
		{
			MoneyConverter converter = new(_rates, account.OnBalance);
			IList<Money> moneyOnAccount = converter.ConvertTo(_currencyCodesToConvertBalanceTo);
			var moneyWithNamedCurrencies = _currencyHelper.ToMoneyWithNamedCurrency(moneyOnAccount);

			balances.Add(new UserAccountInCurrencies(account.MaskedPan, moneyWithNamedCurrencies));
		}

		return balances;
	}

}