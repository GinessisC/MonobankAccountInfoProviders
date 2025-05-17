using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Services.Extensions;
using MonoAccountProvider.Services.Models;
using MonoAccountProvider.Services.Services;

namespace MonoAccountProvider.Services.Repos;

public class CurrencyInfoRepository : ICurrencyInfoRepository
{
	private readonly CurrencyDataReceiverService _service;

	public CurrencyInfoRepository(CurrencyDataReceiverService service)
	{
		_service = service;
	}

	public async IAsyncEnumerable<Currency> GetAllCurrenciesAsync()
	{
		var currenciesDto = _service.GetCurrenciesAsync();
		var currencies = currenciesDto.Select(x => x.ToCurrency());
		
		await foreach (var dto in currencies)
		{
			yield return dto;

		}
	}
}