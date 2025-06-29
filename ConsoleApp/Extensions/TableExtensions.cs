using ConsoleApp.View;
using ConsoleTables;

namespace ConsoleApp.Extensions;

public static class TableExtensions
{
	public static async Task AddAsync(this ConsoleTable table, JarRow jarRow)
	{
		IAsyncEnumerable<string> rawJar = jarRow.GetRawDataAsync();
		object[] rowToAddToTable = await rawJar.ToArrayAsync<object>();

		table.AddRow(rowToAddToTable);
	}

	public static async Task AddAsync(this ConsoleTable table, AccountRow accountRow)
	{
		IAsyncEnumerable<string> rawAccount = accountRow.GetRawDataAsync();
		object[] rowToAddToTable = await rawAccount.ToArrayAsync<object>();

		table.AddRow(rowToAddToTable);
	}
}