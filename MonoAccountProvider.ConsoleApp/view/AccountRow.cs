using System.Globalization;
using MonoAccountProvider.Domain.Entities;

namespace MonoAccountProvider.ConsoleApp.view;

public class AccountRow
{
	private readonly UserAccountInCurrencies _account;
	private readonly UserConfig _userConfig;

	public AccountRow(UserAccountInCurrencies account, UserConfig userConfig)
	{
		_account = account;
		_userConfig = userConfig;
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
		foreach (string currencyName in _userConfig.CurrencyNames)
		{
			decimal balanceInCurrency = await _account.Balance
				.Where(m => m.CurrencyName.ToString() == currencyName)
				.Select(m => m.Amount)
				.FirstOrDefaultAsync();

			yield return balanceInCurrency.ToString(CultureInfo.CurrentCulture);
		}
	}
}