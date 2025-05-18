using System.ComponentModel;
using MonoAccountProvider.ConsoleApp.view;
using MonoAccountProvider.Domain.Entities;

namespace MonoAccountProvider.Tests;

public class PresentationTests
{
	private static readonly IEnumerable<MoneyWithNamedCurrency> _onBalance =
	[
		new(1, "USD")
	];

	private static readonly UserAccountInCurrencies _account = new("1234****5678", _onBalance.ToAsyncEnumerable());
	private static readonly UserJarInCurrencies _jar = new("jar1", _onBalance.ToAsyncEnumerable());

	private static readonly UserConfig _config = new("default_token", ["USD"]);

	[Fact]
	[DisplayName("Finds out what account data will be displayed in the table")]
	public async Task AccountRowDataTest()
	{
		AccountRow accountRow = new(_account, _config);

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
		JarRow accountRow = new(_jar, _config);

		IAsyncEnumerable<string> rawAccount = accountRow.GetRawDataAsync();
		bool maskedPanIsPresented = await rawAccount.ContainsAsync("jar1");
		bool amountOfMoneyIsPresented = await rawAccount.ContainsAsync("1");
		Assert.True(maskedPanIsPresented);
		Assert.True(amountOfMoneyIsPresented);
	}
}