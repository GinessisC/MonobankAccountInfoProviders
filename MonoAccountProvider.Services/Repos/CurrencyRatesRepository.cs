using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Services.Extensions;
using MonoAccountProvider.Services.Models;
using MonoAccountProvider.Services.Services;

namespace MonoAccountProvider.Services.Repos;

public class CurrencyRatesRepository : IRatesRepository
{
	private readonly MonobankRatesService _service;

	public CurrencyRatesRepository(MonobankRatesService service)
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