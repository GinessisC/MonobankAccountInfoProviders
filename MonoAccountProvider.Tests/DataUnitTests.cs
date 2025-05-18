using System.ComponentModel;
using System.Text.Json;
using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Services.Repos;
using MonoAccountProvider.Services.Services;
using NSubstitute;

namespace MonoAccountProvider.Tests;

public class DataUnitTests
{
	[Fact]
	[DisplayName("Checks if it is thrown ArgumentException when token is invalid")]
	public async Task InvalidToken()
	{
		//Arrange
		IAppConfigReader appConfigReader = Substitute.For<IAppConfigReader>();
		appConfigReader.GetSectionAsync("ProfileUri").Returns("https://api.monobank.ua/personal/client-info");

		string invalidToken = "token";
		HttpClient client = new();
		MonobankProfileService service = new(client, appConfigReader);

		//Act and Assert
		await Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetProfileAsync(invalidToken));
	}

	[Fact]
	public async Task GetValidDataFromCurrencyReceiverService()
	{
		//Arrange
		IAppConfigReader appConfigReader = Substitute.For<IAppConfigReader>();

		appConfigReader.GetSectionAsync("CurrencyDataUri")
			.Returns("https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");

		HttpClient client = new();

		CurrencyInfoReceiverService service = new(client, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		}, appConfigReader);

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
		IAppConfigReader appConfigReader = Substitute.For<IAppConfigReader>();
		appConfigReader.GetSectionAsync("CurrencyRatesUri").Returns("https://api.monobank.ua/bank/currency");
		HttpClient client = new();

		CurrencyRatesService service = new(client, new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		}, appConfigReader);

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