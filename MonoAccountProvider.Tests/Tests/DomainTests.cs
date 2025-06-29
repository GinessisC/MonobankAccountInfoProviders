using System.ComponentModel;
using Application.Helpers;
using Application.Repositories;
using Application.Services;
using Domain.Entities;
using Domain.Entities.DataSources;
using Microsoft.Extensions.Options;
using MonoAccountProvider.Domain.Helpers.Interfaces;
using MonoAccountProvider.Domain.Repositories;
using NSubstitute;

namespace MonoAccountProvider.Tests.Tests;

public class DomainTests
{
	private readonly CancellationTokenSource _cts = new();

	private static readonly IEnumerable<Money> _moneyToConvert = new Money[]
	{
		new(1, 840)
	};

	private static readonly IEnumerable<Account> _accounts = new Account[]
	{
		new("1234****5678", new Money(1, 840))
	};

	private static readonly IEnumerable<Jar> _jars = new Jar[]
	{
		new("1234****5678", new Money(1, 840))
	};

	private static readonly Profile _profile = new(_accounts, _jars);

	private readonly IEnumerable<Currency> _defaultCurrencies =
	[
		new("USD", 840),
		new("EUR", 978),
		new("UAH", 980)
	];

	private readonly List<CurrencyRate> _rates =
	[
		new(840, 980, 41.17m, 41.6563m, null),

		new(978, 980, 47.3m, 48.151m, null)
	];

	[Fact]
	[DisplayName("Converts money from one currency to another. Returns list of money of another currency")]
	public async Task ConvertMoneyCorrectly()
	{
		//Arrange
		ICurrencyInfoRepository currencyRepository = Substitute.For<ICurrencyInfoRepository>();
		currencyRepository.GetAllCurrenciesAsync().Returns(_defaultCurrencies.ToAsyncEnumerable());


		IProfileRepository profileRepository = Substitute.For<IProfileRepository>();
		profileRepository.GetProfileAsync(_cts.Token).Returns(_profile);

		ICurrencyOperator currencyOperator = new CurrencyOperator(currencyRepository);

		IRatesRepository ratesRepository = Substitute.For<IRatesRepository>();
		ratesRepository.GetCurrencyRatesAsync().Returns(_rates.ToAsyncEnumerable());

		string[] currencyNamesToConvertTo = ["UAH"];

		UserCfgOptions userCfgOptions = new()
		{
			Token = "Token",
			CurrencyNames = currencyNamesToConvertTo
		};
		IOptions<UserCfgOptions> userOptions = Substitute.For<IOptions<UserCfgOptions>>();
		userOptions.Value.Returns(userCfgOptions);

		AccountData accountData = new(profileRepository, currencyOperator, ratesRepository, userOptions);

		//Act
		IAsyncEnumerable<UserAccountInCurrencies> account = accountData.GetAccountsAsync(_cts.Token);

		IAsyncEnumerable<MoneyWithNamedCurrency> allMoney = MoneyInAccountWhereMaskedPanIs("1234****5678", account);

		MoneyWithNamedCurrency convertedToUah = (await
			allMoney.FirstOrDefaultAsync(m => m.CurrencyName == currencyNamesToConvertTo[0]))!;

		//Assert 
		Assert.Equal(41.17m, convertedToUah.Amount);
	}

	private async IAsyncEnumerable<MoneyWithNamedCurrency> MoneyInAccountWhereMaskedPanIs(
		string maskedPan,
		IAsyncEnumerable<UserAccountInCurrencies> account)
	{
		IAsyncEnumerable<MoneyWithNamedCurrency>? allMoney = await account
			.Where(a => a.MaskedPan == maskedPan)
			.Select(a => a.Balance)
			.FirstOrDefaultAsync();

		await foreach (MoneyWithNamedCurrency money in allMoney)
			yield return money;
	}
}