using Application.Services;
using ConsoleApp.View.Interfaces;
using ConsoleTables;
using Microsoft.Extensions.Options;
using ConsoleApp.Extensions;
using Domain.Entities;
using Domain.Entities.DataSources;

namespace ConsoleApp.View;

public class JarRowsAdder : ITableRowsAdder
{
	private readonly IJarData _jarSource;
	private readonly UserCfgOptions _userCfgOptions;

	public JarRowsAdder(IJarData jarSource, IOptions<UserCfgOptions> userCfgOptions)
	{
		_jarSource = jarSource;
		_userCfgOptions = userCfgOptions.Value;
	}

	public async Task AddToTableAsync(ConsoleTable table, CancellationToken ct)
	{
		var jars = _jarSource.GetJars(ct);
		if (jars == null)
		{
			Console.WriteLine("No Jars found");

			return;
		}

		await foreach (UserJarInCurrencies jar in jars)
		{
			JarRow row = new(jar, _userCfgOptions);
			await table.AddAsync(row);
		}
	}
}