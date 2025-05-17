using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MonoAccountProvider.Services.Repos;

namespace MonoAccountProvider.ConsoleApp.App.Repositories;

public class AppSettingsReader : IAppConfigReader
{
	private readonly IConfiguration _configuration;

	public AppSettingsReader(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public string GetSectionAsync(string sectionName)
	{
		string? uri = _configuration.GetValue<string>(sectionName);

		if (uri is null)
		{
			throw new JsonException($"No parameter called {sectionName} was found.");
		}

		return uri;
	}
}