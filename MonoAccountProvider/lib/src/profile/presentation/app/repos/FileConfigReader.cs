using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MonoAccountProvider.lib.src.profile.domain.Entities;
using MonoAccountProvider.lib.src.profile.domain.repos;

namespace MonoAccountProvider.lib.src.profile.presentation.app.repos;

public class FileConfigReader : IConfigReader
{
	public UserConfig Read()
	{
		IConfigurationBuilder? binder = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json");

		IConfigurationRoot conf = binder.Build();

		string? token = conf.GetValue<string>("Token");
		string[]? currencies = conf.GetSection("Currencies").Get<string[]>();

		if (token is null || currencies is null)
		{
			throw new JsonException("Token and Currencies are required.");
		}

		return new UserConfig(token, currencies);
	}
}