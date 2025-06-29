using System.ComponentModel;
using System.Text.Json;
using Domain.Entities;
using Domain.Entities.DataSources;
using Microsoft.Extensions.Options;
using MonoAccountProvider.Services.Repositories;
using MonoAccountProvider.Services.Services;
using NSubstitute;

namespace MonoAccountProvider.Tests.Tests;

public class DataTests
{
	CancellationTokenSource cts = new();

	private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
	{
		PropertyNameCaseInsensitive = true
	};
	[Fact]
	[DisplayName("Checks if it is thrown ArgumentException when token is invalid")]
	public async Task InvalidToken()
	{
		//Arrange
		string invalidToken = "token";
		HttpClient client = new();
		IOptions<ProfileSourceOptions> profileSourceOp = Substitute.For<IOptions<ProfileSourceOptions>>();
		profileSourceOp.Value.Returns(new ProfileSourceOptions
		{
			Uri = "https://api.monobank.ua/personal/client-info"
		});
		
		IOptionsSnapshot<UserCfgOptions> userOp = Substitute.For<IOptionsSnapshot<UserCfgOptions>>();
		userOp.Value.Returns(new UserCfgOptions() {Token = invalidToken});

		//Act
		MonobankProfileService service = new(client, profileSourceOp, userOp);

		//Assert
		await Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetProfileAsync(cts.Token));
	}

	[Fact]
	public async Task GetValidDataFromCurrencyReceiverService()
	{
		//Arrange
		HttpClient client = new();
		IOptions<CurrencyInfoSourceOptions> currencyInfoSourceOp = Substitute.For<IOptions<CurrencyInfoSourceOptions>>();
		currencyInfoSourceOp.Value.Returns(new CurrencyInfoSourceOptions
		{
			Uri = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json"
		});
		CurrencyInfoReceiverService service = new(client, _jsonOptions, currencyInfoSourceOp);

		CurrencyInfoRepository repo = new(service);

		//Act
		IAsyncEnumerable<Currency> currencies = repo.GetAllCurrenciesAsync();
	
		bool allPropsNeitherNullNorEmpty =
			await currencies.AllAsync(c => string.IsNullOrEmpty(c.Name) is false && c.Code != 0);

		//Assert
		Assert.True(allPropsNeitherNullNorEmpty);
	}

	[Fact]
	public async Task GetValidDataFromRatesService()
	{
		//Arrange
		HttpClient client = new();
		IOptions<RatesSourceOptions> ratesSourceOp = Substitute.For<IOptions<RatesSourceOptions>>();
		ratesSourceOp.Value.Returns(new RatesSourceOptions{Uri = "https://api.monobank.ua/bank/currency"});

		CurrencyRatesService service = new(client, _jsonOptions, ratesSourceOp);

		CurrencyRatesRepository repo = new(service);

		//Act
		IAsyncEnumerable<CurrencyRate> rates = repo.GetCurrencyRatesAsync();
		bool allRatesAreValid = await RatesContainNeededInfoAsync(rates);

		//Assert
		Assert.True(allRatesAreValid);
	}

	private async Task<bool> RatesContainNeededInfoAsync(IAsyncEnumerable<CurrencyRate> rates)
	{
		return await rates.AllAsync(rate => (rate.RateBuy != null && rate.RateSell != null)
			|| rate.RateCross != null);
	}
}