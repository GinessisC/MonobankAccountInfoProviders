using System.ComponentModel;
using MonoAccountProvider.lib.src.profile.domain.cache.Interfaces;
using MonoAccountProvider.lib.src.profile.domain.helpers;
using MonoAccountProvider.lib.src.profile.domain.usecases;
using NSubstitute;

namespace MonoAccountProvider.Tests;

public class DomainUnitTests
{
	private readonly List<Currency> _defaultCurrencies =
	[
		new("USD", 840),
		new("EUR", 978),
		new("UAH", 980)
	];

	private readonly List<CurrencyRate> _rates =
	[
		new(840, 980)
		{
			RateBuy = 41.17m,
			RateSell = 41.6563m
		},

		new(978, 980)
		{
			RateBuy = 47.3m,
			RateSell = 48.151m
		}
	];

	private readonly Profile _profile = new()
	{
		Accounts = new List<Account>
		{
			new(1, 840, "1234****5678")
		}
	};

	[Fact]
	[DisplayName("Converts money from one currency to another. Returns list of money of another currency")]
	public void ConvertMoneyCorrectly()
	{
		//Arrange
		ICurrencyInfoCache currencyCache = Substitute.For<ICurrencyInfoCache>();
		IUserCache userCache = Substitute.For<IUserCache>();

		string[] currencyNamesToConvertTo = ["UAH"];
		UserConfig userConfig = new("default_token", currencyNamesToConvertTo);

		currencyCache.Currencies.Returns(_defaultCurrencies);
		currencyCache.Rates.Returns(_rates);

		CurrencyHelper currencyHelper = new(currencyCache);

		userCache.Profile.Returns(_profile);
		userCache.Conf.Returns(userConfig);

		//Act
		AccountData accountData = new(currencyCache, userCache, currencyHelper);
		IList<UserAccountInCurrencies> account = accountData.GetAccounts();

		IList<MoneyWithNamedCurrency> allMoney = MoneyInAccountWhereMaskedPanIs("1234****5678", account);

		MoneyWithNamedCurrency convertedToUah =
			allMoney.FirstOrDefault(m => m.CurrencyName == currencyNamesToConvertTo[0])!;

		//Assert 
		Assert.Equal(41.17m, convertedToUah.Amount);
	}

	private IList<MoneyWithNamedCurrency> MoneyInAccountWhereMaskedPanIs(string maskedPan,
		IList<UserAccountInCurrencies> account)
	{
		IList<MoneyWithNamedCurrency>? allMoney = account
			.Where(a => a.MaskedPan == maskedPan)
			.Select(a => a.Balance).FirstOrDefault()!;

		return allMoney;
	}
}