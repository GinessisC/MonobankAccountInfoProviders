using ConsoleTables;
using MonoAccountProvider.ConsoleApp.Extensions;
using MonoAccountProvider.ConsoleApp.view.Interfaces;
using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.Repositories;
using MonoAccountProvider.Domain.UseCases;

namespace MonoAccountProvider.ConsoleApp.view;

public class JarRowsAdder : ITableRowsAdder
{
	private readonly IAsyncEnumerable<UserJarInCurrencies>? _jars;
	private readonly UserConfig _userConfig;

	public JarRowsAdder(IJarData jar, UserConfig userConfig)
	{
		_jars = jar.GetJars();
		_userConfig = userConfig;
	}

	public async Task AddToTableAsync(ConsoleTable table)
	{
		if (_jars == null)
		{
			Console.WriteLine("No Jars found");

			return;
		}

		await foreach (UserJarInCurrencies jar in _jars)
		{
			JarRow row = new(jar, _userConfig);
			await table.AddAsync(row);
		}
	}
}