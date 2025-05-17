using System.Net.Http.Json;
using System.Text.Json;
using MonoAccountProvider.Services.Models;
using MonoAccountProvider.Services.Repos;

namespace MonoAccountProvider.Services.Services;

public class CurrencyDataReceiverService
{
	private const int UahCurrencyCode = 980;
	private const string UahCurrencyName = "UAH";
	private readonly IAppConfigReader _appConfigReader;
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonSerializerOptions;

	public CurrencyDataReceiverService(HttpClient httpClient,
		JsonSerializerOptions jsonSerializerOptions,
		IAppConfigReader appConfigReader)
	{
		_httpClient = httpClient;
		_jsonSerializerOptions = jsonSerializerOptions;
		_appConfigReader = appConfigReader;
	}

	public async IAsyncEnumerable<CurrencyDto> GetCurrenciesAsync()
	{
		string currencyDataUri = _appConfigReader.GetSectionAsync("CurrencyDataUri");

		IAsyncEnumerable<CurrencyDto>? currencyDto = await _httpClient.GetFromJsonAsync<IAsyncEnumerable<CurrencyDto>>(
			currencyDataUri,
			_jsonSerializerOptions);

		if (currencyDto is null)
		{
			throw new NullReferenceException();
		}

		await foreach (CurrencyDto currency in currencyDto)
			yield return currency;

		yield return
			new CurrencyDto(UahCurrencyName, UahCurrencyCode); //service does not provide info about uah currency
	}
}