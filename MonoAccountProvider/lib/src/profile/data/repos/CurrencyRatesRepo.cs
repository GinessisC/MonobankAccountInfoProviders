using MonoAccountProvider.lib.src.profile.data.datasource;
using MonoAccountProvider.lib.src.profile.data.Models;
using MonoAccountProvider.lib.src.profile.domain.Entities;
using MonoAccountProvider.lib.src.profile.domain.repos;

namespace MonoAccountProvider.lib.src.profile.data.repos;

public class CurrencyRatesRepo : IRatesRepo
{
	private readonly MonobankRatesService _service;

	public CurrencyRatesRepo(MonobankRatesService service)
	{
		_service = service;
	}

	public async Task<IList<CurrencyRate>> GetCurrencyRatesAsync()
	{
		IList<CurrencyRateDto> ratesDto = await _service.GetCurrenciesRatesAsync();
		IEnumerable<CurrencyRate> rates = ratesDto.Select(r => r.FromDto());

		return rates.ToList();
	}
}