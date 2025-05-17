using System.Globalization;
using ConsoleTables;
using MonoAccountProvider.ConsoleApp.Extensions;
using MonoAccountProvider.ConsoleApp.view.Interfaces;
using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Domain.UseCases;

namespace MonoAccountProvider.ConsoleApp.view;

public class ConsolePresenter
{
	private readonly IEnumerable<ITableRowsAdder> _tableAdders;
	private readonly UserConfig _userConfig;
	public ConsolePresenter(IEnumerable<ITableRowsAdder> tableAdders, UserConfig userConfig)
	{
		_tableAdders = tableAdders;
		_userConfig = userConfig;
	}
	public async Task Present()
	{
		ConsoleTable table = new("Name");
		
		table.AddColumn(_userConfig.CurrencyNames);
		
		foreach (var tableAdder in _tableAdders)
		{
			await tableAdder.AddToTableAsync(table);
		}
		
		table.Write();
	}
}