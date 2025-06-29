using Application.Repositories;
using Domain.Entities;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Services.Mappings;
using MonoAccountProvider.Services.Models;
using MonoAccountProvider.Services.Services;

namespace MonoAccountProvider.Services.Repositories;

public class CurrencyInfoRepository : ICurrencyInfoRepository
{
	private readonly CurrencyInfoReceiverService _service;

	public CurrencyInfoRepository(CurrencyInfoReceiverService service)
	{
		_service = service;
	}

	public async IAsyncEnumerable<Currency> GetAllCurrenciesAsync()
	{
		IAsyncEnumerable<CurrencyDto> currenciesDto = _service.GetCurrenciesAsync();
		IAsyncEnumerable<Currency> currencies = currenciesDto.Select(x => x.ToCurrency());

		await foreach (Currency dto in currencies)
			yield return dto;
	}
}