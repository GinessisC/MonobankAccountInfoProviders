using System.Globalization;
using Application.Services;
using ConsoleApp.View.Interfaces;
using ConsoleTables;
using Domain.Entities;
using Domain.Entities.DataSources;
using Microsoft.Extensions.Options;

namespace ConsoleApp.View;

public class TotalRowAdder : ITableRowsAdder
{
	private readonly IAccountData _accountSource;
	private readonly IJarData _jarSource;
	private readonly UserCfgOptions _userCfgOptions;
	public TotalRowAdder(IOptions<UserCfgOptions> userCfgOptions,
		IAccountData accountSource,
		IJarData jarData)
	{
		_userCfgOptions = userCfgOptions.Value;
		_accountSource = accountSource;
		_jarSource = jarData;
	}

	public async Task AddToTableAsync(ConsoleTable table, CancellationToken ct)
	{
		object[] totalSectionRowToAdd = await GetTotalRowAsync(ct).ToArrayAsync<object>(ct);
		table.AddRow(totalSectionRowToAdd);
	}

	private async IAsyncEnumerable<string> GetTotalRowAsync(CancellationToken ct)
	{
		IAsyncEnumerable<string> totalsInString = GetTotalOnAccountAndJarInAllCurrencies(ct)
			.Select(d => d.ToString(CultureInfo.CurrentCulture));

		yield return "Total";

		await foreach (string total in totalsInString)
			yield return total;
	}

	private async IAsyncEnumerable<decimal> GetTotalOnAccountAndJarInAllCurrencies(CancellationToken ct)
	{
		foreach (string currency in _userCfgOptions.CurrencyNames)
		{
			decimal sumOnAccounts = await GetAmountOfMoneyOnAccountsAsyncIn(currency, ct);
			decimal sumOnJars = await GetAmountOfMoneyOnJarsAsyncIn(currency, ct);

			yield return sumOnAccounts + sumOnJars;
		}
	}

	private async Task<decimal> GetAmountOfMoneyOnAccountsAsyncIn(string currency, CancellationToken ct)
	{
		decimal sum = 0;
		var accounts = _accountSource.GetAccountsAsync(ct);
		await foreach (UserAccountInCurrencies account in accounts)
		{
			decimal currentAccountAmount = await account.Balance
				.Where(m => m.CurrencyName.ToString() == currency)
				.Select(m => m.Amount)
				.FirstOrDefaultAsync();

			sum += currentAccountAmount;
		}

		return sum;
	}

	private async Task<decimal> GetAmountOfMoneyOnJarsAsyncIn(string currency, CancellationToken ct)
	{
		decimal sum = 0;
		var jars = _jarSource.GetJars(ct);
		if (jars == null)
		{
			return sum;
		}

		await foreach (UserJarInCurrencies jar in jars)
		{
			decimal currentJarAmount = await jar.Balance
				.Where(m => m.CurrencyName.ToString() == currency)
				.Select(m => m.Amount)
				.FirstOrDefaultAsync(ct);

			sum += currentJarAmount;
		}

		return sum;
	}
}