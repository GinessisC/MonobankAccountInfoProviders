using ConsoleTables;
using MonoAccountProvider.ConsoleApp.Extensions;
using MonoAccountProvider.ConsoleApp.view.Interfaces;
using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Domain.UseCases;

namespace MonoAccountProvider.ConsoleApp.view;

public class AccountRowsAdder : ITableRowsAdder
{
	private readonly IAsyncEnumerable<UserAccountInCurrencies> _accounts;
	private readonly UserConfig _userConfig;
	public AccountRowsAdder(IAccountData accounts, UserConfig userConfig)
	{
		_accounts = accounts.GetAccountsAsync();
		_userConfig = userConfig;
	}

	public async Task AddToTableAsync(ConsoleTable table)
	{
		await foreach (UserAccountInCurrencies account in _accounts)
		{
			AccountRow row = new(account, _userConfig);
			
			await table.AddAsync(row);
		}
	}
}