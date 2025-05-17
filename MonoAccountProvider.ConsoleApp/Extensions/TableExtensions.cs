using MonoAccountProvider.Domain.Entities;
using ConsoleTables;
using MonoAccountProvider.ConsoleApp.view;

namespace MonoAccountProvider.ConsoleApp.Extensions;

public static class TableExtensions
{
	public static async Task AddAsync(this ConsoleTable table, JarRow jarRow)
	{
		var rawJar = jarRow.GetRawDataAsync();
		var rowToAddToTable = await rawJar.ToArrayAsync<object>();
			
		table.AddRow(rowToAddToTable);
	}
	public static async Task AddAsync(this ConsoleTable table, AccountRow accountRow)
	{
		var rawAccount = accountRow.GetRawDataAsync();
		var rowToAddToTable = await rawAccount.ToArrayAsync<object>();
			
		table.AddRow(rowToAddToTable);
	}
}