using ConsoleApp.View.Interfaces;
using ConsoleTables;
using Domain.Entities.DataSources;
using Microsoft.Extensions.Options;

namespace ConsoleApp.View;

public class ConsolePresenter
{
	private readonly IEnumerable<ITableRowsAdder> _tableAdders;
	private readonly UserCfgOptions _userCfgOptions;

	public ConsolePresenter(IEnumerable<ITableRowsAdder> tableAdders, IOptions<UserCfgOptions> userCfgOptions)
	{
		_tableAdders = tableAdders;
		_userCfgOptions = userCfgOptions.Value;
	}

	public async Task Present(CancellationToken ct)
	{
		ConsoleTable table = new("Name");

		table.AddColumn(_userCfgOptions.CurrencyNames);

		foreach (ITableRowsAdder tableAdder in _tableAdders)
			await tableAdder.AddToTableAsync(table, ct);

		table.Write();
	}
}