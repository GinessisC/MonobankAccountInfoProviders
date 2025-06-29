using Domain.Entities;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Services.Mappings;
using MonoAccountProvider.Services.Models;
using MonoAccountProvider.Services.Services;

namespace MonoAccountProvider.Services.Repositories;

public class CurrencyRatesRepository : IRatesRepository
{
	private readonly CurrencyRatesService _service;

	public CurrencyRatesRepository(CurrencyRatesService service)
	{
		_service = service;
	}

	public async IAsyncEnumerable<CurrencyRate> GetCurrencyRatesAsync()
	{
		IAsyncEnumerable<CurrencyRateDto> ratesDto = _service.GetCurrencyRatesAsync();
		IAsyncEnumerable<CurrencyRate> rates = ratesDto.Select(r => r.ToCurrencyRate());

		await foreach (CurrencyRate rate in rates)
			yield return rate;
	}
}