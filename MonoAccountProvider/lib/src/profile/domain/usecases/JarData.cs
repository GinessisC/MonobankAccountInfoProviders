using MonoAccountProvider.lib.src.profile.domain.cache.Interfaces;
using MonoAccountProvider.lib.src.profile.domain.Entities;
using MonoAccountProvider.lib.src.profile.domain.helpers;
using MonoAccountProvider.lib.src.profile.domain.helpers.Interfaces;

namespace MonoAccountProvider.lib.src.profile.domain.usecases;

public class JarData : IJarData
{
	private readonly IList<Jar>? _jars;
	private readonly IList<int> _currencyCodesToConvertBalanceTo;
	private readonly IList<CurrencyRate> _rates;
	private readonly ICurrencyHelper _currencyHelper;
	public JarData(ICurrencyInfoCache currencyCache, IUserCache userCache, ICurrencyHelper currencyHelper)
	{
		_currencyHelper = currencyHelper;
		_jars = userCache.Profile.Jars;
		_rates = currencyCache.Rates;
		_currencyCodesToConvertBalanceTo =
			currencyHelper.CurrencyCodesThatContainCurrencyNames(userCache.Conf.CurrencyNames);
	}

	public IList<UserJarInCurrencies>? GetJars()
	{
		List<UserJarInCurrencies> balance = new();

		if (_jars is null)
		{
			return null;
		}

		foreach (Jar jar in _jars)
		{
			MoneyConverter converter = new(_rates, jar.OnBalance);
			IList<Money> moneyOnJar = converter.ConvertTo(_currencyCodesToConvertBalanceTo);
			var moneyWithNamedCurrencies = _currencyHelper.ToMoneyWithNamedCurrency(moneyOnJar);
			
			balance.Add(new UserJarInCurrencies(jar.Title, moneyWithNamedCurrencies));
		}

		return balance;
	}
}