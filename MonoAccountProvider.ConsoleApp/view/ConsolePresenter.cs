using ConsoleTables;
using MonoAccountProvider.ConsoleApp.view.Interfaces;
using MonoAccountProvider.Domain.Entities;

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

		foreach (ITableRowsAdder tableAdder in _tableAdders)
			await tableAdder.AddToTableAsync(table);

		table.Write();
	}
}