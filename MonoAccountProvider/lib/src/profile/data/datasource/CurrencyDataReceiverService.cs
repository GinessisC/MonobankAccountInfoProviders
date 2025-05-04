using System.Net.Http.Json;
using System.Text.Json;
using MonoAccountProvider.lib.src.profile.data.Models;

namespace MonoAccountProvider.lib.src.profile.data.datasource;

public class CurrencyDataReceiverService
{
	private const string CurrencyUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";
	private const int UahCurrencyCode = 980;
	private readonly HttpClient _httpClient;

	public CurrencyDataReceiverService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<IList<CurrencyDto>> GetCurrenciesAsync()
	{
		List<CurrencyDto>? currencyDto = await _httpClient.GetFromJsonAsync<List<CurrencyDto>>(CurrencyUrl,
			new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			});

		if (currencyDto is null)
		{
			throw new NullReferenceException();
		}

		currencyDto.Add(new CurrencyDto("UAH", UahCurrencyCode)); //service does not provide info about uah currency

		return currencyDto;
	}
}