using System.Globalization;
using Domain.Entities;
using Domain.Entities.DataSources;

namespace ConsoleApp.View;

public class AccountRow
{
	private readonly UserAccountInCurrencies _account;
	private readonly UserCfgOptions _userCfgOptions;

	public AccountRow(UserAccountInCurrencies account, UserCfgOptions userCfgOptions)
	{
		_account = account;
		_userCfgOptions = userCfgOptions;
	}

	public async IAsyncEnumerable<string> GetRawDataAsync()
	{
		yield return _account.MaskedPan;

		IAsyncEnumerable<string> amountsOfMoney = GetAmountsOfMoneyAsync();

		await foreach (string amount in amountsOfMoney)
			yield return amount;
	}

	private async IAsyncEnumerable<string> GetAmountsOfMoneyAsync()
	{
		foreach (string currencyName in _userCfgOptions.CurrencyNames)
		{
			decimal balanceInCurrency = await _account.Balance
				.Where(m => m.CurrencyName.ToString() == currencyName)
				.Select(m => m.Amount)
				.FirstOrDefaultAsync();

			yield return balanceInCurrency.ToString(CultureInfo.CurrentCulture);
		}
	}
}