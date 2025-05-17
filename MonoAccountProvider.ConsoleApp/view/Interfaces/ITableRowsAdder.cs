using ConsoleTables;

namespace MonoAccountProvider.ConsoleApp.view.Interfaces;

public interface ITableRowsAdder
{
	Task AddToTableAsync(ConsoleTable table);
}