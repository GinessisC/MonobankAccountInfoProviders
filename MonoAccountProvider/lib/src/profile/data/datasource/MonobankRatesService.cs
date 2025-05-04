using System.Net.Http.Json;
using System.Text.Json;
using MonoAccountProvider.lib.src.profile.data.Models;

namespace MonoAccountProvider.lib.src.profile.data.datasource;

public class MonobankRatesService
{
	private const string CurrenciesRatesUrl = "https://api.monobank.ua/bank/currency";
	private readonly HttpClient _httpClient;

	public MonobankRatesService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<IList<CurrencyRateDto>> GetCurrenciesRatesAsync()
	{
		CurrencyRateDto[]? rates = await _httpClient.GetFromJsonAsync<CurrencyRateDto[]>(CurrenciesRatesUrl,
			new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

		if (rates is null)
		{
			throw new NullReferenceException();
		}

		return rates.ToList();
	}
}