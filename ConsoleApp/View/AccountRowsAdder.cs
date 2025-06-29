using Application.Services;
using ConsoleApp.Extensions;
using ConsoleApp.View.Interfaces;
using ConsoleTables;
using Domain.Entities;
using Domain.Entities.DataSources;
using Microsoft.Extensions.Options;

namespace ConsoleApp.View;

public class AccountRowsAdder : ITableRowsAdder
{
	private readonly IAccountData _accountsSource;
	private readonly UserCfgOptions _userCfgOptions;

	public AccountRowsAdder(IAccountData accountsToAdd, IOptions<UserCfgOptions> userCfgOptions)
	{
		_accountsSource = accountsToAdd;
		_userCfgOptions = userCfgOptions.Value;
	}

	public async Task AddToTableAsync(ConsoleTable table, CancellationToken ct)
	{
		IAsyncEnumerable<UserAccountInCurrencies> accounts = _accountsSource.GetAccountsAsync(ct);

		await foreach (UserAccountInCurrencies account in accounts)
		{
			AccountRow row = new(account, _userCfgOptions);

			await table.AddAsync(row);
		}
	}
}