using System.Net.Http.Json;
using System.Text.Json;
using Domain.Entities.DataSources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MonoAccountProvider.Services.Models;

namespace MonoAccountProvider.Services.Services;

public class CurrencyRatesService
{
	private readonly HttpClient _httpClient;
	private readonly JsonSerializerOptions _jsonSerializerOptions;
	private readonly RatesSourceOptions _currencyRatesSourceOptions;

	public CurrencyRatesService(HttpClient client,
		[FromKeyedServices(JsonSerializationOptionNames.PropertyNameCaseInsensitive)]
		JsonSerializerOptions jsonSerializerOptions,
		IOptions<RatesSourceOptions> currencyRatesSourceOptions)
	{
		_httpClient = client;
		_jsonSerializerOptions = jsonSerializerOptions;
		_currencyRatesSourceOptions = currencyRatesSourceOptions.Value;
	}

	public async IAsyncEnumerable<CurrencyRateDto> GetCurrencyRatesAsync()
	{
		IAsyncEnumerable<CurrencyRateDto>? rates =
			await _httpClient.GetFromJsonAsync<IAsyncEnumerable<CurrencyRateDto>>(
				_currencyRatesSourceOptions.Uri,
				_jsonSerializerOptions);

		if (rates is null)
		{
			throw new ArgumentNullException();
		}

		await foreach (CurrencyRateDto rate in rates)
			yield return rate;
	}
}