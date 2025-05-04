using System.ComponentModel;
using MonoAccountProvider.lib.src.profile.data.datasource;
using MonoAccountProvider.lib.src.profile.data.repos;
using MonoAccountProvider.lib.src.profile.domain.Entities;

namespace MonoAccountInfoProviders.Tests;

public class DataUnitTests
{
	[Fact]
	[DisplayName("Checks if it is thrown ArgumentException when token is invalid")]
	public async Task InvalidToken()
	{
		//Arrange
		string invalidToken = "token";
		HttpClient client = new();
		MonobankProfileService service = new(client);

		//Act and Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await service.GetProfileAsync(invalidToken));
	}

	[Fact]
	public async Task GetValidDataFromCurrencyReceiverService()
	{
		//Assert
		HttpClient client = new();
		CurrencyDataReceiverService service = new(client);
		CurrencyDataRepo repo = new(service);

		//Act
		IList<Currency> currencies = await repo.GetAllCurrenciesAsync();
		bool allPropsNeitherNullNorEmpty = currencies.All(c => string.IsNullOrEmpty(c.Name) is false && c.Code != 0);

		//Assert
		Assert.True(allPropsNeitherNullNorEmpty);
	}

	[Fact]
	[DisplayName("Throws exception if token is invalid")]
	public async Task ThrowsExceptionProfileService()
	{
		//Arrange
		HttpClient client = new();
		MonobankProfileService service = new(client);
		ProfileRepo repo = new(service);

		//Act && Assert
		await Assert.ThrowsAsync<ArgumentException>(async () => await repo.GetProfileAsync("incorrect_token"));
	}

	[Fact]
	public async Task GetValidDataFromRatesService()
	{
		//Arrange
		HttpClient client = new();
		MonobankRatesService service = new(client);
		CurrencyRatesRepo repo = new(service);

		//Act
		IList<CurrencyRate> rates = await repo.GetCurrencyRatesAsync();
		bool allRatesAreValid = RatesHaveRatesOfChange(rates);

		//Assert
		Assert.True(allRatesAreValid);
	}

	private bool RatesHaveRatesOfChange(IList<CurrencyRate> rates)
	{
		return rates.All(rate => (rate.RateBuy != null && rate.RateSell != null)
			|| rate.RateCross != null);
	}
}