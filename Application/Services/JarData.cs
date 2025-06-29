using Application.Repositories;
using Domain.Entities;
using Domain.Entities.DataSources;
using Microsoft.Extensions.Options;
using MonoAccountProvider.Domain.Helpers.Interfaces;
using MonoAccountProvider.Domain.Repositories;

namespace Application.Services;

public class JarData : IJarData
{
	private readonly IProfileRepository _profileRepository;
	private readonly IAsyncEnumerable<CurrencyRate> _rates;
	private readonly ICurrencyOperator _currencyOperator;
	private readonly UserCfgOptions _userCfgOptions;

	public JarData(IProfileRepository profileRepository,
		IRatesRepository ratesRepo,
		ICurrencyOperator currencyOperator,
		IOptions<UserCfgOptions> userCfgOptionsReader)
	{
		_profileRepository = profileRepository;
		_rates = ratesRepo.GetCurrencyRatesAsync();
		_currencyOperator = currencyOperator;
		_userCfgOptions = userCfgOptionsReader.Value;
	}

	public async IAsyncEnumerable<UserJarInCurrencies>? GetJars(CancellationToken ct)
	{
		Profile profile = await _profileRepository.GetProfileAsync(ct);

		IAsyncEnumerable<int> targetCurrencyCodes = _currencyOperator.ToCurrencyCodes(_userCfgOptions.CurrencyNames);

		IEnumerable<Jar>? jars = profile.Jars;

		if (jars is null)
		{
			yield break;
		}

		foreach (Jar jar in jars)
		{
			MoneyConverter converter = new(_rates, jar.OnBalance);
			IAsyncEnumerable<Money> moneyOnJar = converter.ConvertTo(targetCurrencyCodes);

			IAsyncEnumerable<MoneyWithNamedCurrency> moneyWithNamedCurrencies =
				_currencyOperator.ToMoneyWithNamedCurrency(moneyOnJar);

			yield return new UserJarInCurrencies(jar.Title, moneyWithNamedCurrencies);
		}
	}
}