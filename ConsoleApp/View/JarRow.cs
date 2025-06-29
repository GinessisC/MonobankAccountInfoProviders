using System.Globalization;
using Domain.Entities;
using Domain.Entities.DataSources;

namespace ConsoleApp.View;

public class JarRow
{
	private readonly UserJarInCurrencies _jar;
	private readonly UserCfgOptions _userCfgOptions;

	public JarRow(UserJarInCurrencies jar,
		UserCfgOptions userCfgOptions)
	{
		_jar = jar;
		_userCfgOptions = userCfgOptions;
	}

	public async IAsyncEnumerable<string> GetRawDataAsync()
	{
		yield return _jar.Title;

		IAsyncEnumerable<string> amountsOfMoney = GetAmountsOfMoneyAsync();

		await foreach (string money in amountsOfMoney)
			yield return money;
	}

	private async IAsyncEnumerable<string> GetAmountsOfMoneyAsync()
	{
		foreach (string currencyName in _userCfgOptions.CurrencyNames)
		{
			decimal balanceInCurrency = await _jar.Balance
				.Where(m => m.CurrencyName.ToString() == currencyName)
				.Select(m => m.Amount)
				.FirstOrDefaultAsync();

			yield return balanceInCurrency.ToString(CultureInfo.CurrentCulture);
		}
	}
}