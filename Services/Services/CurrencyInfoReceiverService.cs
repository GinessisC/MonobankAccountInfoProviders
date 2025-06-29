using System.Net.Http.Json;
using System.Text.Json;
using Domain.Entities.DataSources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MonoAccountProvider.Services.Models;

namespace MonoAccountProvider.Services.Services;

public class CurrencyInfoReceiverService
{
	private const int UahCurrencyCode = 980;
	private const string UahCurrencyName = "UAH";
	private readonly CurrencyInfoSourceOptions _currencyInfoSourceOptions;
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonSerializerOptions;

	public CurrencyInfoReceiverService(HttpClient httpClient,
		[FromKeyedServices(JsonSerializationOptionNames.PropertyNameCaseInsensitive)]
		JsonSerializerOptions jsonSerializerOptions,
		IOptions<CurrencyInfoSourceOptions> currencyInfoSourceOptions)
	{
		_httpClient = httpClient;
		_jsonSerializerOptions = jsonSerializerOptions;
		_currencyInfoSourceOptions = currencyInfoSourceOptions.Value;
	}

	public async IAsyncEnumerable<CurrencyDto> GetCurrenciesAsync()
	{
		IAsyncEnumerable<CurrencyDto>? currencyDto = await _httpClient.GetFromJsonAsync<IAsyncEnumerable<CurrencyDto>>(
			_currencyInfoSourceOptions.Uri,
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