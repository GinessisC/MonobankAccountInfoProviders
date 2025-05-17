using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MonoAccountProvider.Domain.Entities;
using MonoAccountProvider.Domain.Repositories;

namespace MonoAccountProvider.ConsoleApp.App.Repositories;

public class FileUserConfigReader : IUserConfigReader
{
	private readonly IConfiguration _configuration;

	public FileUserConfigReader(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public UserConfig Read()
	{
		string? token = _configuration.GetValue<string>("Token");
		string[]? currencies = _configuration.GetSection("Currencies").Get<string[]>();

		if (token is null || currencies is null)
		{
			throw new JsonException("Token and Currencies are required.");
		}

		return new UserConfig(token, currencies);
	}
}