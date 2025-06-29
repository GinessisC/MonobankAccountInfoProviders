using ConsoleTables;

namespace ConsoleApp.View.Interfaces;

public interface ITableRowsAdder
{
	Task AddToTableAsync(ConsoleTable table, CancellationToken ct);
}