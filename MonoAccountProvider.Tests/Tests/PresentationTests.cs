using System.ComponentModel;
using ConsoleApp.View;
using Domain.Entities;
using Domain.Entities.DataSources;

namespace MonoAccountProvider.Tests.Tests;

public class PresentationTests
{
	private static readonly IEnumerable<MoneyWithNamedCurrency> _onBalance =
	[
		new(1, "USD")
	];

	private static readonly UserAccountInCurrencies _account = new("1234****5678", _onBalance.ToAsyncEnumerable());
	private static readonly UserJarInCurrencies _jar = new("jar1", _onBalance.ToAsyncEnumerable());

	private static readonly UserCfgOptions _cfgOptions = new()
	{
		Token = "token",
		CurrencyNames = ["USD"],
	};

	[Fact]
	[DisplayName("Finds out what account data will be displayed in the table")]
	public async Task AccountRowDataTest()
	{
		AccountRow accountRow = new(_account, _cfgOptions);

		IAsyncEnumerable<string> rawAccount = accountRow.GetRawDataAsync();
		bool maskedPanIsPresented = await rawAccount.ContainsAsync("1234****5678");
		bool amountOfMoneyIsPresented = await rawAccount.ContainsAsync("1");
		Assert.True(maskedPanIsPresented);
		Assert.True(amountOfMoneyIsPresented);
	}

	[Fact]
	[DisplayName("Finds out what jar data will be displayed in the table")]
	public async Task JarRowDataTest()
	{
		JarRow accountRow = new(_jar, _cfgOptions);

		IAsyncEnumerable<string> rawAccount = accountRow.GetRawDataAsync();
		bool maskedPanIsPresented = await rawAccount.ContainsAsync("jar1");
		bool amountOfMoneyIsPresented = await rawAccount.ContainsAsync("1");
		Assert.True(maskedPanIsPresented);
		Assert.True(amountOfMoneyIsPresented);
	}
}