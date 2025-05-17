using MonoAccountProvider.Domain.Entities;

namespace MonoAccountProvider.ConsoleApp.view;

public class JarRow
{
	private readonly UserJarInCurrencies _jar;
	private readonly UserConfig _userConfig;

	public JarRow(UserJarInCurrencies jar,
		UserConfig userConfig)
	{
		_jar = jar;
		_userConfig = userConfig;
	}

	public async IAsyncEnumerable<string> GetRawDataAsync()
	{
		yield return _jar.Title;

		var amountsOfMoney = GetAmountsOfMoneyAsync();
		await foreach (var money in amountsOfMoney)
		{
			yield return money;
		}
	}
	private async IAsyncEnumerable<string> GetAmountsOfMoneyAsync()
	{
		foreach (string currencyName in _userConfig.CurrencyNames)
		{
			decimal balanceInCurrency = await _jar.Balance
				.Where(m => m.CurrencyName.ToString() == currencyName)
				.Select(m => m.Amount)
				.FirstOrDefaultAsync();

			yield return balanceInCurrency.ToString(); //CultureInfo.CurrentCulture - but why? TODO
		}
	}
}