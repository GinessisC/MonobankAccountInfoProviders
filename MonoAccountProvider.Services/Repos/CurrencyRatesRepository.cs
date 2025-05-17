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
		var ratesDto = _service.GetCurrencyRatesAsync();
		var rates = ratesDto.Select(r => r.ToCurrencyRate());

		await foreach (var rate in rates)
		{
			yield return rate;
		}
	}
}