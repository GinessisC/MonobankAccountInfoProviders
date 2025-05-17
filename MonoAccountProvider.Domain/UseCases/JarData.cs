using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.helpers.Interfaces;
using MonoAccountProvider.Domain.Repositories;

namespace MonoAccountProvider.Domain.UseCases;

public class JarData : IJarData
{
	private readonly IProfileRepository _profileRepository;
	private readonly IAsyncEnumerable<CurrencyRate> _rates;
	private readonly ICurrencyOperator _currencyOperator;
	private readonly UserConfig _userConfig;
	public JarData(IProfileRepository profileRepository,
		IRatesRepository ratesRepo,
		ICurrencyOperator currencyOperator,
		IUserConfigReader userConfigReader)
	{
		_profileRepository = profileRepository;
		_rates = ratesRepo.GetCurrencyRatesAsync();
		_currencyOperator = currencyOperator;
		_userConfig = userConfigReader.Read();
	}

	public async IAsyncEnumerable<UserJarInCurrencies>? GetJars()
	{
		var profile = await _profileRepository.GetProfileAsync(_userConfig.Token);
		
		var targetCurrencyCodes = _currencyOperator.ToCurrencyCodes(_userConfig.CurrencyNames);

		var jars = profile.Jars;
		
		if (jars is null)
		{
			yield break;
		}

		foreach (Jar jar in jars)
		{
			MoneyConverter converter = new(_rates, jar.OnBalance);
			IAsyncEnumerable<Money> moneyOnJar = converter.ConvertTo(targetCurrencyCodes);
			var moneyWithNamedCurrencies = _currencyOperator.ToMoneyWithNamedCurrency(moneyOnJar);

			yield return new UserJarInCurrencies(jar.Title, moneyWithNamedCurrencies);
		}
	}
}