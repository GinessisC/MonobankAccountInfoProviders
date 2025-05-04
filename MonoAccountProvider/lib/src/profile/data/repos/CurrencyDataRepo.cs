using MonoAccountProvider.lib.src.profile.data.datasource;
using MonoAccountProvider.lib.src.profile.data.Models;
using MonoAccountProvider.lib.src.profile.domain.Entities;
using MonoAccountProvider.lib.src.profile.domain.repos;

namespace MonoAccountProvider.lib.src.profile.data.repos;

public class CurrencyDataRepo : ICurrencyDataRepo
{
	private readonly CurrencyDataReceiverService _service;

	public CurrencyDataRepo(CurrencyDataReceiverService service)
	{
		_service = service;
	}

	public async Task<IList<Currency>> GetAllCurrenciesAsync()
	{
		IList<CurrencyDto> currenciesDto = await _service.GetCurrenciesAsync();
		IEnumerable<Currency> currencies = currenciesDto.Select(c => c.FromDto());

		return currencies.ToList();
	}
}