using System.Net.Http.Json;
using System.Text.Json;
using MonoAccountProvider.Services.Models;
using MonoAccountProvider.Services.Repos;

namespace MonoAccountProvider.Services.Services;

public class MonobankRatesService
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonSerializerOptions;
	private readonly IAppConfigReader _appConfigReader;
	public MonobankRatesService(HttpClient client,
		JsonSerializerOptions jsonSerializerOptions,
		IAppConfigReader appConfigReader)
	{
		_httpClient = client;
		_jsonSerializerOptions = jsonSerializerOptions;
		_appConfigReader = appConfigReader;
	}

	public async IAsyncEnumerable<CurrencyRateDto> GetCurrencyRatesAsync()
	{
		var currenciesRatesUri = _appConfigReader.GetSectionAsync("CurrencyRatesUri");
		
		var rates = await _httpClient.GetFromJsonAsync<IAsyncEnumerable<CurrencyRateDto>>(
			currenciesRatesUri,
			_jsonSerializerOptions);
		
		if (rates is null)
		{
			throw new ArgumentNullException();
		}
		//Q: why cant I just return IAsyncEnumerable?
		await foreach (var rate in rates)
		{
			yield return rate;
		}
	}
}