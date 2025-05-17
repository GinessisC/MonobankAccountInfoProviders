using ConsoleTables;
using MonoAccountProvider.ConsoleApp.view.Interfaces;
using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.UseCases;

namespace MonoAccountProvider.ConsoleApp.view;

public class TotalRowAdder : ITableRowsAdder
{
	private readonly IAsyncEnumerable<UserAccountInCurrencies> _accounts;
	private readonly IAsyncEnumerable<UserJarInCurrencies>? _jars;
	private readonly UserConfig _userConfig;

	public TotalRowAdder(UserConfig userConfig, IAccountData account, IJarData jarData)
	{
		_userConfig = userConfig;
		_accounts = account.GetAccountsAsync();
		_jars = jarData.GetJars();
	}
	public async Task AddToTableAsync(ConsoleTable table)
	{
		var totalSectionRowToAdd = await GetTotalRowAsync().ToArrayAsync<object>();
		table.AddRow(totalSectionRowToAdd);
	}
	private async IAsyncEnumerable<string> GetTotalRowAsync()
	{
		var totalsInString = GetTotalOnAccountAndJarInAllCurrencies()
			.Select(d => d.ToString());
		
		yield return "Total";
		await foreach (var total in totalsInString)
		{
			yield return total;
		}
	}

	private async IAsyncEnumerable<decimal> GetTotalOnAccountAndJarInAllCurrencies()
	{
		foreach (string currency in _userConfig.CurrencyNames)
		{
			decimal sumOnAccounts = await GetAmountOfMoneyOnAccountsAsyncIn(currency);
			decimal sumOnJars = await GetAmountOfMoneyOnJarsAsyncIn(currency);

			yield return sumOnAccounts + sumOnJars;
		}
	}

	private async Task<decimal> GetAmountOfMoneyOnAccountsAsyncIn(string currency)
	{
		decimal sum = 0;

		await foreach (UserAccountInCurrencies account in _accounts)
		{
			decimal currentAccountAmount = await account.Balance
				.Where(m => m.CurrencyName.ToString() == currency)
				.Select(m => m.Amount)
				.FirstOrDefaultAsync();
			
			sum += currentAccountAmount;
		}

		return sum;
	}

	private async Task<decimal> GetAmountOfMoneyOnJarsAsyncIn(string currency)
	{
		decimal sum = 0;
	
		if (_jars == null)
		{
			return sum;
		}
	
		await foreach (UserJarInCurrencies jar in _jars)
		{
			decimal currentJarAmount = await jar.Balance
				.Where(m => m.CurrencyName.ToString() == currency)
				.Select(m => m.Amount)
				.FirstOrDefaultAsync();
	
			sum += currentJarAmount;
		}
	
		return sum;
	}
}