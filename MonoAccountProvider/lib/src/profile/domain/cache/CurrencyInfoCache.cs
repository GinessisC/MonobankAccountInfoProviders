using MonoAccountProvider.lib.src.profile.domain.cache.Interfaces;
using MonoAccountProvider.lib.src.profile.domain.Entities;
using MonoAccountProvider.lib.src.profile.domain.repos;

namespace MonoAccountProvider.lib.src.profile.domain.cache;

public class CurrencyInfoCache : ICurrencyInfoCache
{
	public IList<CurrencyRate> Rates { get; }
	public IList<Currency> Currencies { get; }

	private CurrencyInfoCache(IList<CurrencyRate> rates, IList<Currency> currencyCodes)
	{
		Rates = rates;
		Currencies = currencyCodes;
	}

	public static async Task<CurrencyInfoCache> Create(IRatesRepo ratesRepo, ICurrencyDataRepo dataRepo)
	{
		IList<CurrencyRate> rates = await ratesRepo.GetCurrencyRatesAsync();
		IList<Currency> codes = await dataRepo.GetAllCurrenciesAsync();

		return new CurrencyInfoCache(rates, codes);
	}
}